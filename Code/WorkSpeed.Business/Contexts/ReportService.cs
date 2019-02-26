using System;
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
        #region Fields
        
        public const string THRESHOLD_FILE = "thresholds.bin";

        private readonly IProductivityBuilder _productivityBuilder;

        private ObservableCollection< ShiftGrouping > _shiftGroupingCollection;
        private ObservableCollection< EmployeeProductivity > _employeeProductivityCollections;

        private ObservableCollection< Appointment > _appointmentCollection;
        private ObservableCollection< Rank > _rankCollection;
        private ObservableCollection< Position > _positionCollection;
        private ObservableCollection< Shift > _shiftCollection;
        private ObservableCollection< ShortBreakSchedule > _shortBreakCollection;
        private ObservableCollection< Operation > _operationCollection;
        private ObservableCollection< Category > _categoryCollection;

        private readonly IFormatter _formatter = new BinaryFormatter();
        private OperationThresholds _thresholds;

        private readonly object _lock = new object();

        #endregion


        #region Ctor

        public ReportService ( WorkSpeedDbContext dbContext, IProductivityBuilder builder ) : base( dbContext )
        {
            _productivityBuilder = builder ?? throw new ArgumentNullException(nameof(builder), @"IProductivityBuilder cannot be null.");
            
            CreateCollections();
            SetupPeriod();

            void CreateCollections ()
        {
            _employeeProductivityCollections = new ObservableCollection< EmployeeProductivity >();
            EmployeeProductivityCollections = new ReadOnlyObservableCollection< EmployeeProductivity >( _employeeProductivityCollections );

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

            _categoryCollection = new ObservableCollection< Category >();
            CategoryCollection = new ReadOnlyObservableCollection< Category >( _categoryCollection );
        }

            void SetupPeriod ()
        {
            var now = DateTime.Now;
            var start = now.Date.Subtract( TimeSpan.FromDays( now.Day - 1 ) );
            Period = new Period( start, now );
        }
        }

        #endregion


        #region Properties
        
        public Period Period { get; set; }

        public ReadOnlyObservableCollection< EmployeeProductivity > EmployeeProductivityCollections { get; private set; }
        public ReadOnlyObservableCollection< Category > CategoryCollection { get; private set; }
        public ReadOnlyObservableCollection< ShiftGrouping > ShiftGroupingCollection { get; private set; }

        public ReadOnlyObservableCollection< Appointment > AppointmentCollection { get; private set; }
        public ReadOnlyObservableCollection< Rank > RankCollection { get; private set; }
        public ReadOnlyObservableCollection< Position > PositionCollection { get; private set; }
        public ReadOnlyObservableCollection< Shift > ShiftCollection { get; private set; }
        public ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakCollection { get; private set; }
        public ReadOnlyObservableCollection< Operation > OperationCollection { get; private set; }

        #endregion


        #region Methods

        public void SetPeriodAsync ()
        {
            throw new NotImplementedException();
        }

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

            if ( typeof( T ).IsAssignableFrom( typeof( Category ) ) ) {
                ReloadCollection( _categoryCollection, DbContext.GetCategories() );
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( ShiftGrouping ) ) ) {
                await LoadShiftGroupingAsync();
                return;
            }

            if ( typeof( T ).IsAssignableFrom( typeof( EmployeeProductivity ) ) ) {
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
            ReloadEntities< Category >();
        }

        public void SetPeriodAsync ( Period period )
        {
            throw new NotImplementedException();
        }

        public OperationThresholds GetThresholds ()
        {
            if ( _thresholds == null ) {
                ReadThresholds();
                _thresholds.ThresholdChanged += OnThresholdChanged;
            }

            return _thresholds;
        }

        public Task LoadShiftGroupingAsync ()
        {
            var tcs = new TaskCompletionSource< bool >();

            Task.Run( () => { 
                try {
                    LoadShiftGrouping();
                    tcs.SetResult( true );
                }
                catch ( Exception ) {

                    if ( _shiftGroupingCollection.Any() ) { _shiftGroupingCollection.Clear(); }
                    tcs.SetResult( false );
                }
            });

            return tcs.Task;
        }

        private void LoadShiftGrouping ()
        {
            lock ( _lock ) {
                if ( _shiftGroupingCollection.Any() ) {
                    _shiftGroupingCollection.Clear();
                }

                foreach ( var grouping in _dbContext.GetShiftGrouping().Select( s => new ShiftGrouping( s.shift, s.appointments ) ) ) {
                    _shiftGroupingCollection.Add( grouping );
                }
            }
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

        /// <summary>
        /// Writes thresholds to the file.
        /// </summary>
        public void WriteThresholds ()
        {
            using ( FileStream file = File.Create( THRESHOLD_FILE ) ) {
                _formatter.Serialize( file, _thresholds );
            }
        }


        protected void OnThresholdChanged ( object threshold, EventArgs args )
        {
            WriteThresholds();
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

        private IEnumerable< EmployeeProductivity > GetProductivityCollection ( Period period )
        {
            var actions = _dbContext.GetEmployeeActions( period.Start, period.End ).ToArray();
            if ( actions.Length == 0 ) yield break;

            _productivityBuilder.BuildNew();
            _productivityBuilder.Thresholds = _thresholds;

            foreach ( IGrouping< Employee, EmployeeActionBase > grouping in actions ) {

                var breaks = grouping.Key.ShortBreakSchedule;
                var shift = grouping.Key.Shift;

                yield return new EmployeeProductivity( grouping.Key, GetProductivities( grouping, breaks, shift ) );
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
