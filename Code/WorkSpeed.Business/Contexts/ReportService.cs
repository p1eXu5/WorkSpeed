﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Contexts.Productivity.Builders;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Context.ReportService;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts
{
    public class ReportService : Service, IReportService
    {
        public const string THRESHOLD_FILE = "thresholds.bin";

        private readonly IProductivityBuilder _productivityBuilder;

        private readonly ObservableCollection< EmployeeProductivityCollection > _employeeProductivities;
        private readonly ObservableCollection< ShiftGrouping > _shiftGroupings;
        private readonly ObservableCollection< Shift > _shifts;
        private readonly ObservableCollection< ShortBreakSchedule > _shortBreakSchedules;

        private readonly IFormatter _formatter = new BinaryFormatter();
        private OperationThresholds _thresholds;


        #region Ctor

        public ReportService ( WorkSpeedDbContext dbContext, IProductivityBuilder builder ) : base( dbContext )
        {
            _productivityBuilder = builder ?? throw new ArgumentNullException(nameof(builder), @"IProductivityBuilder cannot be null.");

            AppointmentCollection = _dbContext.Appointments.ToArray();
            RankCollection = _dbContext.Ranks.ToArray();
            PositionCollection = _dbContext.Positions.ToArray();

            _shifts = new ObservableCollection< Shift >( _dbContext.Shifts );
            ShiftCollection = new ReadOnlyObservableCollection< Shift >( _shifts );

            _shortBreakSchedules = new ObservableCollection< ShortBreakSchedule >( _dbContext.ShortBreakSchedules );
            ShortBreakCollection = new ReadOnlyObservableCollection< ShortBreakSchedule >( _shortBreakSchedules );
        }

        #endregion


        public IEnumerable< Appointment > AppointmentCollection { get; }
        public IEnumerable< Rank > RankCollection { get; }
        public IEnumerable< Position > PositionCollection { get; }


        public ReadOnlyObservableCollection< Shift > ShiftCollection { get; }
        public ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakCollection { get; }

        public IEnumerable< ShiftGrouping > ShiftGroupingCollection { get; private set; }

        #region Methods

        public Task LoadEmployeesAsync ()
        {
            var tcs = new TaskCompletionSource< bool >();

            Task.Run( () => { 
                try {
                    _shiftGroupings.Clear();
                    foreach ( var grouping in _dbContext.GetShiftGrouping().Select( s => new ShiftGrouping( s.shift, s.appointments ) ) ) {
                        _shiftGroupings.Add( grouping );
                    }
                }
                catch ( Exception ) {

                    _shiftGroupings.Clear();
                    tcs.SetResult( false );
                }
            });

            return tcs.Task;
        }

        public Task GetProductivityAsync ( Period period )
        {
            var tcs = new TaskCompletionSource< bool >();

            Task.Run( () =>
                      {
                          try {
                              _employeeProductivities.Clear();
                              foreach ( var collection in GetProductivityCollection( period ) ) {
                                  _employeeProductivities.Add( collection );
                              }
                              tcs.SetResult( true );
                          }
                          catch ( Exception ) {
                              _employeeProductivities.Clear();
                              tcs.SetResult( false );
                          }
                      }
                    );

            return tcs.Task;
        }


        private IEnumerable< EmployeeProductivityCollection > GetProductivityCollection ( Period period )
        {
            var actions = _dbContext.GetEmployeeActions( period.Start, period.End ).ToArray();
            if ( actions.Length == 0 ) yield break;

            _productivityBuilder.BuildNew();
            _productivityBuilder.Thresholds = _thresholds;

            foreach ( IGrouping< Employee, EmployeeActionBase > grouping in actions ) {

                var breaks = grouping.Key.ShortBreakSchedule;
                var shift = grouping.Key.Shift;

                yield return new EmployeeProductivityCollection( grouping.Key, GetProductivities( grouping, breaks, shift ) );
            }
        }


        private (IReadOnlyDictionary< Operation, IProductivity >,HashSet< Period >) GetProductivities ( IEnumerable< EmployeeActionBase > actions, ShortBreakSchedule breaks, Shift shift )
        {
            var actionArr = actions.ToArray();
            if ( actionArr.Length == 0 ) { return (new Dictionary< Operation, IProductivity >(0), new HashSet< Period >()); }

            _productivityBuilder.BuildNew();
            var current = _productivityBuilder.CheckDuration( actionArr[ 0 ] );
            var next = current;

            for ( int i = 1; i < actionArr.Length; ++i ) {

                current = _productivityBuilder.CheckDuration( actionArr[ i ] );
                next =  _productivityBuilder.CheckPause( current, next );
            }

            _productivityBuilder.SubstractBreaks( breaks );
            _productivityBuilder.SubstractLunch( shift );

            return _productivityBuilder.GetResult();
        }

        public OperationThresholds GetThresholds ()
        {
            if ( _thresholds == null ) {
                ReadThresholds();
                _thresholds.ThresholdChanged += OnThresholdChanged;
            }

            return _thresholds;
        }

        protected void OnThresholdChanged ( object threshold, EventArgs args )
        {
            WriteThresholds();
        }

        private void ReadThresholds ()
        {
            if ( !File.Exists( "thresholds.bin" ) ) {

                _thresholds =  GetNewThresholds();
                WriteThresholds();
                return;
            }

            using ( var stream = File.OpenRead( THRESHOLD_FILE ) ) {
                stream.Position = 0;
                _thresholds = ( OperationThresholds )_formatter.Deserialize( stream );
            }
        }

        /// <summary>
        /// Writes thresholds to the file.
        /// </summary>
        public void WriteThresholds ()
        {
            using ( FileStream file = File.Create( THRESHOLD_FILE ) ) {
                _formatter.Serialize( file, _thresholds );
            }
        }

        /// <summary>
        /// Invokes if thresholds have not created and have not saved in file.
        /// </summary>
        /// <returns></returns>
        private OperationThresholds GetNewThresholds ()
        {
            var operations = _dbContext.GetOperations().ToArray();
            var thresholds = new OperationThresholds(  );

            foreach ( var operation in operations ) {
                thresholds[ operation ] = 20;
            }

            return thresholds;
        }
        
        #endregion
    }
}
