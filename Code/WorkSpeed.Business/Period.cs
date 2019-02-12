using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public static Period Zero => new Period(new DateTime(0), new DateTime(0));

        public TimeSpan Duration => End - Start;

        public bool Contains ( Period period )
        {
            if ( period.Start >= Start && period.End <= End ) {
                return true;
            }

            return false;
        }

        public bool IsTheSameDate ( Period other )
        {
            return Start.Date == other.Start.Date;
        }

        public static bool operator < ( Period periodA, Period periodB )
        {
            return (periodA.Start < periodB.Start
                    && periodA.End < periodB.End);
        }

        public static bool operator > ( Period periodA, Period periodB )
        {
            return (periodA.Start > periodB.Start
                    && periodA.End > periodB.End);
        }

        public static Period operator - ( Period periodA, Period periodB )
        {
            if ( periodB.Start >= periodA.End || periodB.End <= periodA.Start ) return periodA;
            if ( periodB.Start <= periodA.Start && periodB.End >= periodA.End ) return Period.Zero;

            if ( periodB.Start >= periodA.Start && periodB.Start < periodA.End ) {

                if ( periodB.End <= periodA.End ) {

                    periodA.End = periodA.End.Subtract( periodB.Duration );
                    return periodA;
                }

                if ( periodB.End > periodA.End ) {

                    periodA.End = periodB.Start;
                    return periodA;
                }
            }

            if ( periodB.End > periodA.Start && periodB.End <= periodA.End ) {

                if ( periodB.Start < periodA.Start ) {

                    periodA.Start = periodB.End;
                    return periodA;
                }

                if ( periodB.Start >= periodA.Start ) {

                    periodA.End = periodA.End.Subtract( periodB.Duration );
                    return periodA;
                }
            }

            return periodA;
        }
    }
}
