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
        public ReportService ( WorkSpeedDbContext dbContext ) : base( dbContext )
        {
            Appointments = _dbContext.Appointments.ToArray();
            Ranks = _dbContext.Ranks.ToArray();
            Positions = _dbContext.Positions.ToArray();
        }

        public Appointment[] Appointments { get; }
        public Rank[] Ranks { get; }
        public Position[] Positions { get; }

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
