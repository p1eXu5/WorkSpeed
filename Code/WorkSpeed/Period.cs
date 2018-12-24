using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed
{
    public struct Period
    {
        public Period (DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Period Zero => new Period(new DateTime(0), new DateTime(0));

        public bool Contains ( Period period )
        {
            if ( period.Start >= Start && period.End <= End ) {
                return true;
            }

            return false;
        }
    }
}
