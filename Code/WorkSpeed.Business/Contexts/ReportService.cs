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
        public ReportService ( WorkSpeedDbContext dbContext ) : base( dbContext ) { }

        public ShiftGrouping[] Shifts { get; private set; }

        public void LoadEmployees ()
        {
            try {
                Shifts = _dbContext.GetShiftGrouping().Select( s => new ShiftGrouping( s.shift, s.appointments ) ).ToArray();
            }
            catch ( Exception ) {
                Shifts = new ShiftGrouping[0];
            }
        }
    }
}
