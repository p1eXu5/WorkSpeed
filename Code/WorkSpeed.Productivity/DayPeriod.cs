﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Productivity
{
    public struct DayPeriod
    {
        public DayPeriod ( TimeSpan start, TimeSpan end )
        {
            Start = start;
            End = end;
        }

        public TimeSpan Start { get; }
        public TimeSpan End { get; }

        public TimeSpan Duration => End - Start;

        public bool IsIntersect ( DayPeriod other )
        {
            return (other.Start >= Start && other.Start <= End)
                   || (other.End >= Start && other.End <= End);
        }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals( null, obj ) ) return false;
            return obj is DayPeriod other && Equals( other );
        }

        public bool Equals ( DayPeriod other )
        {
            return other.Start == Start && other.End == End;
        }

        public override int GetHashCode ()
        {
            return Start.GetHashCode() + 31 * End.GetHashCode();
        }

        public static bool operator == ( DayPeriod dayPeriodA, DayPeriod dayPeriodB )
        {
            return dayPeriodA.Equals( dayPeriodB );
        }

        public static bool operator != ( DayPeriod dayPeriodA, DayPeriod dayPeriodB )
        {
            return !dayPeriodA.Equals( dayPeriodB );
        }
    }
}