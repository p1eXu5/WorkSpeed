using System;
using System.Collections.Generic;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public static class DataModelExtensions
    {
        public static Queue< Period > GetBreaks ( this ShortBreakSchedule shortBreak, DateTime downtime )
        {
            if (shortBreak == null) throw new ArgumentNullException(nameof(shortBreak), @"ShortBreak cannot be null");

            var queue = new Queue< Period >();
            var breaksEnd = downtime.Date.Add( shortBreak.FirstBreakTime );
            var startPeriod = downtime;

            var breakStart = breaksEnd.Subtract( TimeSpan.FromDays( 1 ) );

            do {
                var period = new Period( breakStart, breakStart.Add( shortBreak.Duration ) );
                queue.Enqueue( period );

                if ( breakStart < startPeriod ) {
                    startPeriod = breakStart;
                }
                else {
                    queue.Enqueue( queue.Dequeue() );
                }

                breakStart = breakStart.Add( shortBreak.Duration );


            } while ( breakStart < breaksEnd );

            return queue;
        }
    }
}
