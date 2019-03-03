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

        private readonly double _periodicity;

        public ICollection< (TimeSpan start, TimeSpan end) > Breaks => _breaks;
        public TimeSpan BreakDuration => _shortBreakSchedule.Duration;
        public TimeSpan FirstBreakTime => _shortBreakSchedule.FirstBreakTime;

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

            var count =(int)( MinPerDay / shortBreaks.Periodicity.TotalMinutes );
            _breaks = new (TimeSpan start, TimeSpan end)[ count ];

            _periodicity = shortBreaks.Periodicity.TotalMinutes;
            var dur = shortBreaks.Duration.TotalMinutes;

            var breakStart = shortBreaks.FirstBreakTime.TotalMinutes % _periodicity;

            for ( int i = 0; i < count; ++i ) {

                var end = breakStart + dur;
                if ( end >= MinPerDay ) { end -= MinPerDay; }

                _breaks[i] = ( (TimeSpan.FromMinutes( breakStart ), TimeSpan.FromMinutes( end )) );
                breakStart += _periodicity;
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
            var periodEnd = period.End.TimeOfDay;
            var mid = (_breaks.Length ) / 2;
            var diff = mid;

            while ( mid > 0 && mid < (_breaks.Length - 1) ) {

                if ( (periodEnd >= _breaks[ mid ].end || ( periodEnd >= _breaks[ mid ].start && periodEnd < _breaks[ mid ].end ) ) 
                     && periodEnd < _breaks[ mid + 1 ].end ) {
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


            if ( mid == 0 ) {
                if ( (periodEnd >= _breaks[ mid ].end || ( periodEnd >= _breaks[ mid ].start && periodEnd < _breaks[ mid ].end ) ) 
                    && periodEnd < _breaks[ mid + 1 ].end ) {
                    return new ShortBreakInspectorMomento( 
                                   new Period( 
                                       new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[mid].start.Hours, _breaks[mid].start.Minutes, _breaks[mid].start.Seconds ), 
                                       new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[mid].end.Hours, _breaks[mid].end.Minutes, _breaks[mid].end.Seconds )
                                   ) 
                               );
                }

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

            if ( _breaks[ mid ].end < _breaks[ mid ].start ) {

                return new ShortBreakInspectorMomento( 
                           new Period(
                               new DateTime( period.End.Year, period.End.Month, period.End.Day, _breaks[ mid ].start.Hours, _breaks[ mid ].start.Minutes, _breaks[ mid ].start.Seconds ),
                               new DateTime( period.End.Year, period.End.Month, period.End.Day + 1, _breaks[ mid ].end.Hours, _breaks[ mid ].end.Minutes, _breaks[ mid ].end.Seconds )
                           ) 
                       );
            }

            return new ShortBreakInspectorMomento( 
                           new Period(
                               new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, _breaks[ mid ].start.Hours, _breaks[ mid ].start.Minutes, _breaks[ mid ].start.Seconds ),
                               new DateTime( period.End.Year, period.End.Month, period.End.Day - 1, _breaks[ mid ].end.Hours, _breaks[ mid ].end.Minutes, _breaks[ mid ].end.Seconds )
                           ) 
                       );
        }

        /// <summary>
        ///     Checks intersaction, move break to the next period.
        /// </summary>
        /// <param name="downtime">Downtime with duration less than break duration.</param>
        /// <param name="momento"></param>
        /// <returns></returns>
        public bool IsBreak ( Period downtime, ShortBreakInspectorMomento momento )
        {
            if ( downtime.IsIntersectsWith( momento.Break ) ) {
                momento.LockDeposit();
                return true;
            }

            if ( downtime.Start > momento.Break.End ) {

                if ( !momento.IsDepositLocked() ) {
                    momento.UnlockDeposit();
                }
                else {
                    momento.SetDeposit();
                }
                Next( downtime, momento );
                return IsBreak( downtime, momento );
            }

            return false;
        }

        /// <summary>
        ///     Move break to the next period, sets momento deposit.
        /// </summary>
        /// <param name="nextDowntime"></param>
        /// <param name="momento"></param>
        public void Next ( Period nextDowntime, ShortBreakInspectorMomento momento )
        {

            int distance = (int)((nextDowntime.End - momento.Break.End).TotalMinutes / _periodicity);
            
            if ( distance == 0 ) {
                // move next
                momento.Break = new Period( 
                                        momento.Break.Start.Add( _shortBreakSchedule.Periodicity ), 
                                        momento.Break.End.Add( _shortBreakSchedule.Periodicity ) 
                                    );
            }
            else {
                // move distance
                momento.Break = new Period( 
                                        momento.Break.Start.Add( TimeSpan.FromMinutes( _periodicity * distance ) ), 
                                        momento.Break.End.Add( TimeSpan.FromMinutes( _periodicity * distance ) ) 
                                    );

                if ( distance == 1 ) {
                    
                }
            }
        }

        public Period GetPreviousBreak ( ShortBreakInspectorMomento momento )
        {
            return new Period( 
                            momento.Break.Start.Subtract( _shortBreakSchedule.Periodicity ), 
                            momento.Break.End.Subtract( _shortBreakSchedule.Periodicity ) 
                        );
        }

    }
}
