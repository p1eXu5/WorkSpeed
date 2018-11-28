using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Manager
{
    public struct Period
    {
        public Period (DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }

        public Period Zero => new Period(new DateTime(0), new DateTime(0));
    }
}
