using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class ShortBreakInspector : IShortBreakInspector
    {
        private const double MinPerDay = 1440;
        private readonly (TimeSpan start, TimeSpan end)[] _breaks;
        private readonly ShortBreakSchedule _shortBreakSchedule;

        public ICollection< (TimeSpan start, TimeSpan end) > Breaks => _breaks;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortBreaks"></param>
        public ShortBreakInspector ( ShortBreakSchedule shortBreaks )
        {
            if (shortBreaks == null) { throw new ArgumentNullException(nameof(shortBreaks), @"ShortBreakSchedule cannot be null."); }

            if ( shortBreaks.Periodicity.TotalMinutes > (8 * 60)
                 || shortBreaks.Duration.TotalMinutes > 15
                 || (int)shortBreaks.FirstBreakTime.TotalDays > 0) { throw new ArgumentException(@"shortBreaks wrong.", nameof(shortBreaks)); }

            _shortBreakSchedule = shortBreaks;

            var count =(int)Math.Floor( MinPerDay / shortBreaks.Periodicity.TotalMinutes );
            _breaks = new (TimeSpan start, TimeSpan end)[ count ];

            var breakPeriodicity = shortBreaks.Periodicity.TotalMinutes;
            var dur = shortBreaks.Duration.TotalMinutes;

            var breakStart = shortBreaks.FirstBreakTime.TotalMinutes % breakPeriodicity;

            for ( int i = 0; i < count; ++i ) {

                var end = breakStart + dur;
                if ( end >= MinPerDay ) { end -= MinPerDay; }

                _breaks[i] = ( (TimeSpan.FromMinutes( breakStart ), TimeSpan.FromMinutes( end )) );
                breakStart += breakPeriodicity;
                if ( breakStart >= MinPerDay ) { breakStart -= MinPerDay; }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public ShortBreakInspectorMomento SetBreak ( Period period )
        {
            if (period.Duration < _shortBreakSchedule.Duration) { throw new ArgumentException(@"perion duration wrong.", nameof(period.Duration)); }

            var periodEnd = period.End.TimeOfDay;
            var mid = (_breaks.Length ) / 2;
            var diff = mid;

            while ( mid > 0 && mid < (_breaks.Length - 1) ) {

                if ( periodEnd > _breaks[ mid ].end && periodEnd < _breaks[ mid + 1 ].end ) {
                    return new ShortBreakInspectorMomento( 
                                    new Period( 
                                        new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[mid].start.Hours, _breaks[mid].start.Minutes, _breaks[mid].start.Seconds ), 
                                        new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[mid].end.Hours, _breaks[mid].end.Minutes, _breaks[mid].end.Seconds )
                                    )
                                );
                }

                if ( diff > 1 ) { diff /= 2; }

                if ( periodEnd > _breaks[ mid ].end ) {
                    mid += diff;
                }
                else {
                    mid -= diff;
                }
            }

            if ( periodEnd > _breaks[ mid ].end && periodEnd < _breaks[ mid + 1 ].end ) {
                return new ShortBreakInspectorMomento( 
                               new Period( 
                                   new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[mid].start.Hours, _breaks[mid].start.Minutes, _breaks[mid].start.Seconds ), 
                                   new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[mid].end.Hours, _breaks[mid].end.Minutes, _breaks[mid].end.Seconds )
                               ) 
                           );
            }

            if ( mid == 0 ) {

                var lastBreak = _breaks[ _breaks.Length - 1 ];

                if ( lastBreak.end < lastBreak.start ) {

                    return new ShortBreakInspectorMomento( 
                                   new Period(
                                       new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, lastBreak.start.Hours, lastBreak.start.Minutes, lastBreak.start.Seconds ),
                                       new DateTime( period.End.Year, period.End.Month, period.End.Day, lastBreak.end.Hours, lastBreak.end.Minutes, lastBreak.end.Seconds )
                                   ) 
                               );
                }

                return new ShortBreakInspectorMomento( 
                               new Period(
                                   new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, lastBreak.start.Hours, lastBreak.start.Minutes, lastBreak.start.Seconds ),
                                   new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, lastBreak.end.Hours, lastBreak.end.Minutes, lastBreak.end.Seconds )
                               ) 
                           );
            }

            return new ShortBreakInspectorMomento( 
                           new Period(
                               new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, _breaks[0].start.Hours, _breaks[0].start.Minutes, _breaks[0].start.Seconds ),
                               new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, _breaks[0].end.Hours, _breaks[0].end.Minutes, _breaks[0].end.Seconds )
                           ) 
                       );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="period"></param>
        /// <param name="momento"></param>
        /// <returns></returns>
        public bool IsBreak ( Period period, ShortBreakInspectorMomento momento )
        {
            throw new NotImplementedException();
        }

    }
}
