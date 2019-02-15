using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.DataContexts.ReportService;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts
{
    public class ReportService : Service
    {
        private readonly ObservableCollection< Shift > _shifts;
        private readonly ObservableCollection< ShortBreakSchedule > _shortBreakSchedules;

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

        public Appointment[] Appointments { get; }
        public Rank[] Ranks { get; }
        public Position[] Positions { get; }

        public ReadOnlyObservableCollection< Shift > Shifts { get; set; }
        public ReadOnlyObservableCollection< ShortBreakSchedule > ShortBreakSchedules { get; set; }

        public ShiftGrouping[] ShiftGrouping { get; private set; }

        public void LoadEmployees ()
        {
            try {
                ShiftGrouping = _dbContext.GetShiftGrouping().Select( s => new ShiftGrouping( s.shift, s.appointments ) ).ToArray();
            }
            catch ( Exception ) {
                ShiftGrouping = new ShiftGrouping[0];
            }
        }
    }
}
