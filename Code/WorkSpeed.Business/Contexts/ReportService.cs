using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity.Builders;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Context.ReportService;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts
{
    public class ReportService : Service
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

            Appointments = _dbContext.Appointments.ToArray();
            Ranks = _dbContext.Ranks.ToArray();
            Positions = _dbContext.Positions.ToArray();

            _shifts = new ObservableCollection< Shift >( _dbContext.Shifts );
            Shifts = new ReadOnlyObservableCollection< Shift >( _shifts );

            _shortBreakSchedules = new ObservableCollection< ShortBreakSchedule >( _dbContext.ShortBreakSchedules );
            ShortBreakSchedules = new ReadOnlyObservableCollection< ShortBreakSchedule >( _shortBreakSchedules );
        }

        #endregion


        public Appointment[] Appointments { get; }
        public Rank[] Ranks { get; }
        public Position[] Positions { get; }


        public ReadOnlyObservableCollection< EmployeeProductivityCollection > EmployeeProductivities { get; }
        public ReadOnlyObservableCollection< ShiftGrouping > ShiftGroupings { get; }
        public ReadOnlyObservableCollection< Shift > Shifts { get; }
        public ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakSchedules { get; }

        public ShiftGrouping[] ShiftGrouping { get; private set; }

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
                              foreach ( var productivity in GetProductivity( period ) ) {
                                  _employeeProductivities.Add( productivity );
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

        private IEnumerable<EmployeeProductivityCollection> GetProductivity ( Period period )
        {
            var actions = _dbContext.GetEmployeeActions( period.Start, period.End ).ToArray();

            _productivityBuilder.Thresholds = _thresholds;

            foreach ( IGrouping< Employee, EmployeeActionBase > grouping in actions ) {

                var employeeProductivities = new EmployeeProductivityCollection( grouping.Key );
                var breaks = grouping.Key.ShortBreakSchedule;
                var shift = grouping.Key.Shift;

                employeeProductivities.Productivities = GetProductivities( grouping, breaks, shift );
                
                yield return employeeProductivities;
            }
        }


        private IReadOnlyDictionary< Operation, IProductivity > GetProductivities ( IEnumerable< EmployeeActionBase > actions, ShortBreakSchedule breaks, Shift shift )
        {
            EmployeeActionBase nextAction = null;

            foreach ( var action in actions ) {

                _productivityBuilder.CheckDuration( action );

                if ( nextAction == null ) {
                    nextAction = action;
                }
                else {
                    _productivityBuilder.CheckPause( action, nextAction );
                    nextAction = action;
                }
            }

            _productivityBuilder.SubstractBreaks( breaks );
            _productivityBuilder.SubstractLunch( shift );

            return _productivityBuilder.Productivities;
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
