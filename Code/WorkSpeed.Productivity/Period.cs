using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Productivity;

namespace WorkSpeed
{
    public struct Period
    {
        public Period ( DateTime start, DateTime end )
        {
            if ( start > end ) throw new ArgumentException("Start can't be greater than End.");

            Start = start;
            End = end;
        }

        public DateTime Start { get; }
        public DateTime End { get; }

        public static Period Zero => new Period(new DateTime(0), new DateTime(0));

        public TimeSpan Duration => End - Start;

        public DayPeriod GetDayPeriod ()
        {
            return new DayPeriod( Start.TimeOfDay, End - Start );
        }

        public DateTime[] GetDays ()
        {
            var days = new List< DateTime >();
            var start = Start;

            do {
                days.Add( start.Date );
                start = start.AddDays( 1 );

            } while ( start.Date < End.Date );

            return days.ToArray();
        }

        public bool Contains ( Period other )
        {
            return (other.Start >= Start && other.End <= End);
        }

        public override bool Equals ( object obj )
        {
            if ( ReferenceEquals( null, obj ) ) return false;
            return obj is Period other && Equals( other );
        }

        public bool Equals ( Period other )
        {
            return other.Start == Start && other.End == End;
        }

        public override int GetHashCode ()
        {
            return Start.GetHashCode() + 31 * End.GetHashCode();
        }

        public static bool operator == ( Period periodA, Period periodB )
        {
            return periodA.Equals( periodB );
        }

        public static bool operator != ( Period periodA, Period periodB )
        {
            return !( periodA == periodB );
        }

        public static bool operator < ( Period periodA, Period periodB )
        {
            return (periodA.Start < periodB.Start
                    && periodA.End < periodB.Start);
        }

        public static bool operator > ( Period periodA, Period periodB )
        {
            return (periodA.Start > periodB.End
                    && periodA.End > periodB.End);
        }
    }
}
