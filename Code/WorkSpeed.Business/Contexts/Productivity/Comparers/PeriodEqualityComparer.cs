using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Productivity.Comparers
{
    class PeriodEqualityComparer : IEqualityComparer< Period >
    {
        public bool Equals ( Period x, Period y )
        {
            return y.Start >= x.Start && y.End <= x.End;
        }

        public int GetHashCode ( Period obj )
        {
            throw new NotImplementedException();
        }
    }
}
