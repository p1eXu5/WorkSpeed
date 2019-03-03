using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public struct Period : IEqualityComparer< Period >
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

        public bool Contains ( Period other )
        {
            return (other.Start >= Start && other.End <= End);
        }

        /// <summary>
        /// Returns true if duration of period greater than interval.
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public bool Contains ( TimeSpan interval )
        {
            return Duration >= interval;
        }

        public Period CutEnd ( TimeSpan interval )
        {
            if ( Contains( interval ) ) {
                return new Period( Start, End.Subtract( interval ) );
            }

            return Period.Zero;
        }

        public bool IsIntersectsWith ( Period other )
        {
            return (Start > other.Start && Start < other.End)
                    || ( End > other.Start && End < other.End)
                    || Contains( other );
        }

        public DayPeriod GetDayPeriod ()
        {
            var start = Start.TimeOfDay;
            var end = End.TimeOfDay;

            if ( end - start == TimeSpan.Zero && End > Start ) {
                return new DayPeriod( Start.TimeOfDay, End.TimeOfDay + TimeSpan.FromSeconds( 1 ) );
            }

            return new DayPeriod( start, end );
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

        public bool IsTheSameDate ( Period other )
        {
            return Start.Date == other.Start.Date;
        }

        public DateTime GetMedian ()
        {
            var d = (End - Start).TotalSeconds / 2;
            return Start.Add( TimeSpan.FromSeconds( d ) );
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
            return Start.DayOfYear.GetHashCode() * 1_000_000 + ( (int)Math.Floor(End.TimeOfDay.TotalMilliseconds ) / 100);
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

        public static Period operator - ( Period periodA, Period periodB )
        {
            DateTime newBorder;

            if ( periodB.Start >= periodA.End || periodB.End <= periodA.Start ) return periodA;
            if ( periodB.Start <= periodA.Start && periodB.End >= periodA.End ) return Period.Zero;

            if ( periodB.Start >= periodA.Start && periodB.Start < periodA.End ) {

                //
                //  |___________A___________|
                //                                      
                //        |_____B________|
                //
                //  |__res___|
                //
                if ( periodB.End <= periodA.End ) {

                    newBorder = periodA.End.Subtract( periodB.Duration );
                    return new Period( periodA.Start, newBorder );
                }


                //
                //  |___________A___________|
                //
                //        |_____B______________|
                //
                //  |_res_|
                //
                if ( periodB.End > periodA.End ) {

                    return new Period( periodA.Start, periodB.Start );
                }
            }

            if ( periodB.End > periodA.Start && periodB.End <= periodA.End ) {

                if ( periodB.Start < periodA.Start ) {

                    return new Period( periodB.End, periodA.End );
                }

                if ( periodB.Start >= periodA.Start ) {

                    newBorder = periodA.End.Subtract( periodB.Duration );
                    return new Period( periodA.Start, newBorder );
                }
            }

            return periodA;
        }

        public bool Equals ( Period x, Period y )
        {
            return x.Equals( y );
        }

        public int GetHashCode ( Period obj )
        {
            return obj.GetHashCode();
        }
    }
}
