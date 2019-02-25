using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        private ObservableCollection< ShiftGrouping > _shiftGroupingCollection;
        private ObservableCollection< EmployeeProductivityCollection > _employeeProductivityCollections;

        private ObservableCollection< Appointment > _appointmentCollection;
        private ObservableCollection< Rank > _rankCollection;
        private ObservableCollection< Position > _positionCollection;
        private ObservableCollection< Shift > _shiftCollection;
        private ObservableCollection< ShortBreakSchedule > _shortBreakCollection;
        private ObservableCollection< Operation > _operationCollection;

        private readonly IFormatter _formatter = new BinaryFormatter();
        private OperationThresholds _thresholds;


        #region Ctor

        public ReportService ( WorkSpeedDbContext dbContext, IProductivityBuilder builder ) : base( dbContext )
        {
            _productivityBuilder = builder ?? throw new ArgumentNullException(nameof(builder), @"IProductivityBuilder cannot be null.");
            
            CreateCollections();
            SetupPeriod();
            ReloadAllCollections();
        }

        public void SetPeriodAsync ()
        {
            throw new NotImplementedException();
        }

        private void CreateCollections ()
        {
            _employeeProductivityCollections = new ObservableCollection< EmployeeProductivityCollection >();
            EmployeeProductivityCollections = new ReadOnlyObservableCollection< EmployeeProductivityCollection >( _employeeProductivityCollections );

            _shiftGroupingCollection = new ObservableCollection< ShiftGrouping >();
            ShiftGroupingCollection = new ReadOnlyObservableCollection< ShiftGrouping >( _shiftGroupingCollection );

            _appointmentCollection = new ObservableCollection< Appointment >();
            AppointmentCollection = new ReadOnlyObservableCollection< Appointment >( _appointmentCollection );

            _rankCollection = new ObservableCollection< Rank >();
            RankCollection = new ReadOnlyObservableCollection< Rank >( _rankCollection );

            _positionCollection = new ObservableCollection< Position >();
            PositionCollection = new ReadOnlyObservableCollection< Position >( _positionCollection );

            _shiftCollection = new ObservableCollection< Shift >();
            ShiftCollection = new ReadOnlyObservableCollection< Shift >( _shiftCollection );

            _shortBreakCollection = new ObservableCollection< ShortBreakSchedule >();
            ShortBreakCollection = new ReadOnlyObservableCollection< ShortBreakSchedule >( _shortBreakCollection );

            _operationCollection = new ObservableCollection< Operation >();
            OperationCollection = new ReadOnlyObservableCollection< Operation >( _operationCollection );
        }

        private void SetupPeriod ()
        {
            var now = DateTime.Now;
            var start = now.Date.Subtract( TimeSpan.FromDays( now.Day - 1 ) );
            Period = new Period( start, now );
        }

        #endregion


        public Period Period { get; set; }

        public ReadOnlyObservableCollection< EmployeeProductivityCollection > EmployeeProductivityCollections { get; set; }
        public ReadOnlyObservableCollection< ShiftGrouping > ShiftGroupingCollection { get; set; }

        public ReadOnlyObservableCollection< Appointment > AppointmentCollection { get; set; }
        public ReadOnlyObservableCollection< Rank > RankCollection { get; set; }
        public ReadOnlyObservableCollection< Position > PositionCollection { get; set; }
        public ReadOnlyObservableCollection< Shift > ShiftCollection { get; set; }
        public ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakCollection { get; set; }
        public ReadOnlyObservableCollection< Operation > OperationCollection { get; set; }


        #region Methods

        public async void ReloadEntities< T > ()
        {
            if ( typeof( T ).IsAssignableFrom( typeof( Appointment ) ) ) {
                ReloadCollection( _appointmentCollection, DbContext.GetAppointments() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( Rank ) ) ) {
                ReloadCollection( _rankCollection, DbContext.GetRanks() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( Position ) ) ) {
                ReloadCollection( _positionCollection, DbContext.GetPositions() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( Shift ) ) ) {
                ReloadCollection( _shiftCollection, DbContext.GetShifts() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( ShortBreakSchedule ) ) ) {
                ReloadCollection( _shortBreakCollection, DbContext.GetShortBreakSchedules() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( Operation ) ) ) {
                ReloadCollection( _operationCollection, DbContext.GetOperations() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( ShiftGrouping ) ) ) {
                await LoadShiftGroupingAsync();
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( EmployeeProductivityCollection ) ) ) {
                await LoadEmployeeProductivitiesAsync();
                return;
            }
        }

        public void ReloadAllCollections ()
        {
            ReloadEntities< Appointment >();
            ReloadEntities< Rank >();
            ReloadEntities< Position >();
            ReloadEntities< Shift >();
            ReloadEntities< ShortBreakSchedule >();
            ReloadEntities< Operation >();
        }

        private void ReloadCollection< T > ( ICollection< T > coll, IEnumerable< T > entities )
        {
            if ( coll.Count > 0 ) {
                    coll.Clear();
            }

            foreach ( var entity in entities ) {
                coll.Add( entity );
            }
        }

        public Task LoadShiftGroupingAsync ()
        {
            var tcs = new TaskCompletionSource< bool >();

            Task.Run( () => { 
                try {
                    if ( _shiftGroupingCollection.Any() ) { _shiftGroupingCollection.Clear(); }
                    foreach ( var grouping in _dbContext.GetShiftGrouping().Select( s => new ShiftGrouping( s.shift, s.appointments ) ) ) {
                        _shiftGroupingCollection.Add( grouping );
                    }
                }
                catch ( Exception ) {

                    if ( _shiftGroupingCollection.Any() ) { _shiftGroupingCollection.Clear(); }
                    tcs.SetResult( false );
                }
            });

            return tcs.Task;
        }

        public Task LoadEmployeeProductivitiesAsync ()
        {
            var tcs = new TaskCompletionSource< bool >();

            Task.Run( () =>
                      {
                          try {
                              _employeeProductivityCollections.Clear();
                              foreach ( var collection in GetProductivityCollection( Period ) ) {
                                  _employeeProductivityCollections.Add( collection );
                              }
                              tcs.SetResult( true );
                          }
                          catch ( Exception ) {
                              _employeeProductivityCollections.Clear();
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
