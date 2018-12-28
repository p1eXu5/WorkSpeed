using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Productivity.Comparers;

namespace WorkSpeed.Productivity
{
    public class BreakRepository : IBreakRepository
    {
        private readonly Dictionary< Shift, TimeSpan > _variableBreaks;
        private readonly Dictionary< ShortBreak, (Predicate<Employee> predicate, List<DayPeriod> periods) > _fixedBreaks;



        public BreakRepository ()
        {
            _variableBreaks = new Dictionary< Shift, TimeSpan >( 2 );
            _fixedBreaks = new Dictionary< ShortBreak, (Predicate<Employee> predicate, List< DayPeriod > periods) >( 2 );
        }



        public TimeSpan ShortBreakDownLimit { get; private set; } = TimeSpan.FromMinutes( 5 );
        public TimeSpan ShortBreakUpLimit { get; private set; } = TimeSpan.FromMinutes( 10 );

        public TimeSpan ShortBreakIntervalDownLimit { get; private set; } = TimeSpan.FromMinutes( 50 );
        public TimeSpan ShortBreakIntervalUpLimit { get; private set; } = TimeSpan.FromHours( 2 );


        public TimeSpan LongBreakDownLimit { get; private set; } = TimeSpan.FromMinutes( 15 );
        public TimeSpan LongBreakUpLimit { get; private set; } = TimeSpan.FromHours( 2 );

        public TimeSpan ShiftDurationDownLimit { get; private set; } = TimeSpan.FromHours( 4 );
        public TimeSpan ShiftDurationUpLimit { get; private set; } = TimeSpan.FromHours( 12 );


        public IEnumerable< Shift > ShiftCollection => _variableBreaks.Keys;
        public IEnumerable< ShortBreak > ShortBreakCollection => _fixedBreaks.Keys;



        public List< DayPeriod > GetDayPeriods ( ShortBreak shortBreak )
        {
            if ( shortBreak == null ) throw new ArgumentNullException( nameof( shortBreak ), "ShortBreak cannot be null" );

            // Check duration
            if ( shortBreak.Duration < ShortBreakDownLimit || shortBreak.Duration > ShortBreakUpLimit)
                throw new ArgumentException();

            // Check interval
            var interval = shortBreak.Periodicity - shortBreak.Duration;

            if ( interval < ShortBreakIntervalDownLimit || interval > ShortBreakIntervalUpLimit )
                throw new ArgumentException();

            // Check shift
            if ( shortBreak.Shift == null ) throw new ArgumentNullException( nameof( shortBreak ), "ShortBreak.Shift cannot be null" );

            var offset = shortBreak.Shift.StartTime;

            if ( offset < TimeSpan.Zero || offset >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException();

            var dayPeriodList = new List< DayPeriod >();

            TimeSpan start;
            TimeSpan end = offset;
            DayPeriod dayPeriod;

            do
            {
                start = end + interval;
                end += interval + shortBreak.Duration;
                dayPeriod = new DayPeriod( start, end );
                dayPeriodList.Add( dayPeriod );

            } while ( end < TimeSpan.FromDays( 1 ) );

            var lastEnd = end > TimeSpan.FromDays( 1 ) ? end - TimeSpan.FromDays( 1 ) : TimeSpan.Zero;

            end = offset;
            start = end - shortBreak.Duration;

            while ( start > lastEnd )
            {

                dayPeriod = new DayPeriod( start, end );
                dayPeriodList.Add( dayPeriod );
                end = start - interval;
                start = end - shortBreak.Duration;
            }

            return dayPeriodList;
        }

        public void AddFixedBreak ( ShortBreak shortBreak, Predicate< Employee > predicate )
        {
            if ( predicate == null ) throw new ArgumentNullException( nameof( predicate ), "Predicate cannot be null" );
            var dayPerioList = GetDayPeriods( shortBreak );

            _fixedBreaks.Add( shortBreak, (predicate, dayPerioList) );
        }

        public void AddVariableBreak ( Shift shift )
        {
            if ( shift == null ) throw new ArgumentNullException( nameof( shift ), "ShortBreak cannot be null" );

            if ( shift.ShiftDuration < ShiftDurationDownLimit || shift.ShiftDuration > ShiftDurationUpLimit )
                throw new ArgumentException();

            if ( shift.LunchDuration < LongBreakDownLimit || shift.LunchDuration > LongBreakUpLimit )
                throw new ArgumentException();

            if ( shift.StartTime < TimeSpan.Zero || shift.StartTime >= TimeSpan.FromDays( 1 ) )
                throw new ArgumentException();

            var endTime = shift.StartTime + shift.ShiftDuration;

            if ( endTime > TimeSpan.FromDays( 1 ) ) {
                endTime = endTime - TimeSpan.FromDays( 1 );
            }

            _variableBreaks.Add( shift, endTime );
        }

        public ShortBreak CheckShortBreak ( Employee employee, Period period )
        {
            if ( period.Start >= period.End ) {
                return null;
            }

            if ( period.Duration < ShortBreakDownLimit ) return null;

            var shortBreak = _fixedBreaks.Keys
                             .FirstOrDefault( sb => _fixedBreaks[ sb ].predicate( employee ) 
                                                    && sb.Duration <= period.Duration
                                                    && _fixedBreaks[ sb ].periods.Contains( period.GetDayPeriod(), new DayPeriodEqualityComparer() ) );

            return shortBreak;
        }



        public Shift CheckLunchBreak ( Period period )
        {
            if ( period.Start >= period.End ) {
                return null;
            }

            if ( period.Duration < LongBreakDownLimit ) return null;

            var shift = _variableBreaks.Keys.FirstOrDefault( s => period.Start.TimeOfDay > s.StartTime && period.End.TimeOfDay < _variableBreaks[ s ] );

            return shift;
        }

    }
}
