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
        private readonly Dictionary<Shift, DayPeriod> _variableBreaks;
        private readonly Dictionary<ShortBreakSchedule, (Predicate<Employee> predicate, List<DayPeriod> periods)> _fixedBreaks;



        public BreakRepository ()
        {
            _variableBreaks = new Dictionary<Shift, DayPeriod>( 2 );
            _fixedBreaks = new Dictionary<ShortBreakSchedule, (Predicate<Employee> predicate, List<DayPeriod> periods)>( 2 );
        }


        #region Properties

        public TimeSpan ShortBreakDownLimit { get; private set; } = TimeSpan.FromMinutes( 5 );
        public TimeSpan ShortBreakUpLimit { get; private set; } = TimeSpan.FromMinutes( 10 );

        public TimeSpan ShortBreakIntervalDownLimit { get; private set; } = TimeSpan.FromMinutes( 50 );
        public TimeSpan ShortBreakIntervalUpLimit { get; private set; } = TimeSpan.FromHours( 2 );


        public TimeSpan LongBreakDownLimit { get; private set; } = TimeSpan.FromMinutes( 15 );
        public TimeSpan LongBreakUpLimit { get; private set; } = TimeSpan.FromHours( 2 );

        public TimeSpan ShiftDurationDownLimit { get; private set; } = TimeSpan.FromHours( 4 );
        public TimeSpan ShiftDurationUpLimit { get; private set; } = TimeSpan.FromHours( 12 );


        public IEnumerable<Shift> ShiftCollection => _variableBreaks.Keys;
        public IEnumerable<ShortBreakSchedule> ShortBreakCollection => _fixedBreaks.Keys;

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBreak"></param>
        /// <returns></returns>
        public List<DayPeriod> GetDayPeriods ( ShortBreakSchedule shortBreak )
        {
            //if ( shortBreak == null ) throw new ArgumentNullException( nameof( shortBreak ), "ShortBreak cannot be null" );

            //// Check duration
            //if ( shortBreak.Duration < ShortBreakDownLimit || shortBreak.Duration > ShortBreakUpLimit )
            //    throw new ArgumentException();

            //// Check interval
            //var interval = shortBreak.Periodicity - shortBreak.Duration;

            //if ( interval < ShortBreakIntervalDownLimit || interval > ShortBreakIntervalUpLimit )
            //    throw new ArgumentException();

            //var offset = shortBreak.DayOffsetTime;

            //if ( offset < TimeSpan.Zero || offset >= TimeSpan.FromDays( 1 ) )
            //    throw new ArgumentException();

            //var dayPeriodList = new List<DayPeriod>();

            //TimeSpan start;
            //TimeSpan end = offset;
            //DayPeriod dayPeriod;

            //do
            //{
            //    start = end + interval;
            //    end += interval + shortBreak.Duration;

            //    if ( end < TimeSpan.FromDays( 1 ) )
            //    {
            //        dayPeriod = new DayPeriod( start, end );
            //        dayPeriodList.Add( dayPeriod );
            //    }

            //} while ( end < TimeSpan.FromDays( 1 ) );


            //if ( end > TimeSpan.FromDays( 1 ) )
            //{

            //    end = end - TimeSpan.FromDays( 1 );
            //    dayPeriod = new DayPeriod( start, end );
            //    dayPeriodList.Add( dayPeriod );
            //}

            //var lastEnd = end;
            //end = offset;
            //start = end - shortBreak.Duration;

            //while ( start > lastEnd )
            //{

            //    dayPeriod = new DayPeriod( start, end );
            //    dayPeriodList.Add( dayPeriod );
            //    end = start - interval;
            //    start = end - shortBreak.Duration;
            //}

            //return dayPeriodList;

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBreak"></param>
        /// <param name="predicate"></param>
        public void AddFixedBreak ( ShortBreakSchedule shortBreak, Predicate<Employee> predicate )
        {
            if ( predicate == null ) throw new ArgumentNullException( nameof( predicate ), "Predicate cannot be null" );
            var dayPerioList = GetDayPeriods( shortBreak );

            _fixedBreaks.Add( shortBreak, (predicate, dayPerioList) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shift"></param>
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

            if ( endTime > TimeSpan.FromDays( 1 ) )
            {
                endTime = endTime - TimeSpan.FromDays( 1 );
            }

            _variableBreaks.Add( shift, new DayPeriod( shift.StartTime, endTime ) );
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Shift[] CheckLunchBreak ( Period period )
        {
            if ( period.Duration < LongBreakDownLimit ) return new Shift[0];

            var periodDay = period.GetDayPeriod();


            var shift = _variableBreaks.Keys.Where( s => period.Duration >= s.LunchDuration && _variableBreaks[ s ].GetIntersectDuration( periodDay ) >= s.LunchDuration ).ToArray();

            return shift;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public (ShortBreakSchedule shortBreak, TimeSpan breakLength) CheckShortBreak ( Period period, Employee employee )
        {
            if ( employee == null ) throw new ArgumentNullException();

            var shortBreak = _fixedBreaks.Keys
                                         .FirstOrDefault( sb => _fixedBreaks[ sb ].predicate( employee ) );

            if ( shortBreak == null ) return (null, TimeSpan.Zero);

            var pariodDay = period.GetDayPeriod();

            var dayPeriodTimes = _fixedBreaks[ shortBreak ]
                                 .periods.Where( p => p.IsIntersects( pariodDay ) ).ToArray();

            if ( !dayPeriodTimes.Any() ) return (null, TimeSpan.Zero);

            var breakLenght = dayPeriodTimes.Aggregate( TimeSpan.Zero,
                                                       ( span, dayPeriod ) =>
                                                           span += dayPeriod.GetIntersectDuration( pariodDay ) );

            return (shortBreak, breakLenght);
        }


    }
}
