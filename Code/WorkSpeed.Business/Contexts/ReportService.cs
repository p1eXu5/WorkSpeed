using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.DataContexts.ReportService;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts
{
    public class ReportService : Service
    {
        public const string THRESHOLD_FILE = "thresholds.bin";

        private readonly ObservableCollection< Shift > _shifts;
        private readonly ObservableCollection< ShortBreakSchedule > _shortBreakSchedules;
        private readonly IFormatter _formatter = new BinaryFormatter();
        private OperationThresholds _thresholds;

        #region Ctor

        public ReportService ( WorkSpeedDbContext dbContext ) : base( dbContext )
        {
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

        public ReadOnlyObservableCollection< Shift > Shifts { get; set; }
        public ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakSchedules { get; set; }

        public ShiftGrouping[] ShiftGrouping { get; private set; }

        #region Methods

        public Task LoadEmployeesAsync ()
        {
            return Task.Run( () => LoadEmployees() );
        }

        public void LoadEmployees ()
        {
            try {
                ShiftGrouping = _dbContext.GetShiftGrouping().Select( s => new ShiftGrouping( s.shift, s.appointments ) ).ToArray();
            }
            catch ( Exception ) {
                ShiftGrouping = new ShiftGrouping[0];
            }
        }

        public OperationThresholds GetThresholds ()
        {
            if ( _thresholds == null ) {
                ReadThresholds();
                _thresholds.ThresholdChanged += OnThresholdChanged;
            }

            return _thresholds;
        }

        private void OnThresholdChanged ( object threshold, EventArgs args )
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

        public void WriteThresholds ()
        {
            using ( FileStream file = File.Create( THRESHOLD_FILE ) ) {
                _formatter.Serialize( file, _thresholds );
            }
        }

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
