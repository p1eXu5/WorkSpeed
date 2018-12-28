using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Productivity.Comparers
{
    class DayPeriodEqualityComparer : IEqualityComparer<DayPeriod>
    {
        public bool Equals ( DayPeriod x, DayPeriod y )
        {
            return y.Start >= x.Start && y.End <= x.End;
        }

        public int GetHashCode ( DayPeriod obj )
        {
            throw new NotImplementedException();
        }
    }
}
