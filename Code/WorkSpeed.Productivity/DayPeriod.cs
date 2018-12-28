using System;
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
            if ( start >= TimeSpan.FromDays( 1 ) || start < TimeSpan.Zero ) throw new ArgumentException();
            if ( end >= TimeSpan.FromDays( 1 ) || start < TimeSpan.Zero ) throw new ArgumentException();

            Start = start;
            End = end;
        }

        public static DayPeriod Zero => new DayPeriod( TimeSpan.Zero, TimeSpan.Zero );

        public TimeSpan Start { get; }
        public TimeSpan End { get; }

        public TimeSpan GetIntersectDuration ( DayPeriod other )
        {
            if ( other.Start >= TimeSpan.FromDays( 1 ) || other.Start < TimeSpan.Zero ) throw new ArgumentException();
            if ( other.End >= TimeSpan.FromDays( 1 ) || other.End < TimeSpan.Zero ) throw new ArgumentException();

            // 1
            if ( Start > End && other.Start > other.End ) {

                // 1-1
                if ( other.Start <= Start && other.End >= End ) {
                    return (TimeSpan.FromDays( 1 ) - Start) + End;
                }

                // 1-2
                if ( other.Start > Start && other.End >= End ) {
                    return (TimeSpan.FromDays( 1 ) - other.Start) + End;
                }

                // 1-3
                if ( other.Start <= Start && other.End < End )
                {
                    return (TimeSpan.FromDays( 1 ) - Start) + other.End;
                }

                // 1-4
                if ( other.Start > Start && other.End < End )
                {
                    return (TimeSpan.FromDays( 1 ) - other.Start) + other.End;
                }
            }

            // 2
            if ( Start > End && other.Start < other.End )
            {
                // 2-1
                if ( other.Start <= Start && other.End > Start )
                {
                    return other.End - Start;
                }

                // 2-2
                if ( other.Start > Start && other.End > Start )
                {
                    return other.End - other.Start;
                }

                // 2-3
                if ( other.Start < End && other.End >= End )
                {
                    return End - other.Start;
                }

                // 2-4
                if ( other.Start < End && other.End <= End )
                {
                    return other.End + other.Start;
                }
            }

            // 3
            if ( Start < End && other.Start < other.End ) {

                // 3-1
                if ( other.Start <= Start && other.End >= End ) {
                    return End - Start;
                }

                // 3-2
                if ( other.Start > Start && other.Start < End && other.End >= End ) {
                    return End - other.Start;
                }

                // 3-3
                if ( other.Start <= Start && other.End < End && other.End > Start ) {
                    return other.End + Start;
                }

                // 3-4
                if ( other.Start > Start && other.End < End ) {
                    return other.End - other.Start;
                }
            }

            // 4
            if ( Start < End && other.Start > other.End )
            {
                // 4-1
                if ( other.Start <= End && other.End >= Start )
                {
                    return (End - other.Start) + (other.End - Start);
                }

                // 4-2
                if ( other.Start <= End && other.End < Start )
                {
                    return End - other.Start;
                }

                // 4-3
                if ( other.Start > End && other.End >= Start ) {
                    return other.End + Start;
                }
            }

            return TimeSpan.Zero;
        }

        public bool IsIntersects ( DayPeriod other )
        {
            if ( other.Start >= TimeSpan.FromDays( 1 ) || other.Start < TimeSpan.Zero ) throw new ArgumentException();
            if ( other.End >= TimeSpan.FromDays( 1 ) || other.End < TimeSpan.Zero ) throw new ArgumentException();

            // 1
            if ( Start > End && other.Start > other.End )
            {
                // 1-1
                if ( other.Start <= Start && other.End >= End )
                {
                    return true;
                }

                // 1-2
                if ( other.Start > Start && other.End >= End )
                {
                    return true;
                }

                // 1-3
                if ( other.Start <= Start && other.End < End )
                {
                    return true;
                }

                // 1-4
                if ( other.Start > Start && other.End < End )
                {
                    return true;
                }
            }

            // 2
            if ( Start > End && other.Start < other.End )
            {
                // 2-1
                if ( other.Start <= Start && other.End > Start )
                {
                    return true;
                }

                // 2-2
                if ( other.Start > Start && other.End > Start )
                {
                    return true;
                }

                // 2-3
                if ( other.Start < End && other.End >= End )
                {
                    return true;
                }

                // 2-4
                if ( other.Start < End && other.End <= End )
                {
                    return true;
                }
            }

            // 3
            if ( Start < End && other.Start < other.End )
            {
                // 3-1
                if ( other.Start <= Start && other.End >= End )
                {
                    return true;
                }

                // 3-2
                if ( other.Start > Start && other.Start < End && other.End >= End )
                {
                    return true;
                }

                // 3-3
                if ( other.Start <= Start && other.End < End && other.End > Start )
                {
                    return true;
                }

                // 3-4
                if ( other.Start > Start && other.End < End )
                {
                    return true;
                }
            }

            // 4
            if ( Start < End && other.Start > other.End )
            {
                // 4-1
                if ( other.Start <= End && other.End >= Start )
                {
                    return true;
                }

                // 4-2
                if ( other.Start <= End && other.End < Start )
                {
                    return true;
                }

                // 4-3
                if ( other.Start > End && other.End >= Start )
                {
                    return true;
                }
            }

            return false;
        }

        public Period GetDatePeriod ( DateTime dateTime )
        {
            var date = dateTime.Date;

            return new Period( date.Add( Start ), date.Add( End ) );
        }


        public override string ToString ()
        {
            return $"{Start} - {End}";
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
            return !( dayPeriodA == dayPeriodB );
        }
    }
}
