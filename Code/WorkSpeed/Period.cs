using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed
{
    public struct Period
    {
        public Period ( TimeSpan start, TimeSpan end )
        {
            Start = start;
            End = end;
        }

        public TimeSpan Start { get; }
        public TimeSpan End { get; }

        public Period Zero => new Period(new TimeSpan( 0 ), new TimeSpan( 0 ));

        public bool Contains ( Period period )
        {
            if ( period.Start >= Start && period.End <= End ) {
                return true;
            }

            return false;
        }
    }
}
