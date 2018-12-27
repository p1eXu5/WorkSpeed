using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class PauseBetweenActions : IPauseBetweenActions
    {
        private readonly HashSet< DateTime > _breaks;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="breakRepository"></param>
        /// <param name="minRestBetweenShifts">Minimum pause between shifts.</param>
        public PauseBetweenActions ( IBreakRepository breakRepository, TimeSpan minRestBetweenShifts )
        {
            BreakRepository = breakRepository ?? throw new ArgumentNullException( nameof( breakRepository ), "IBreakRepository cannot be null." );
            MinRestBetweenShifts = minRestBetweenShifts;
            _breaks = new HashSet< DateTime >();
        }

        public TimeSpan MinRestBetweenShifts { get; set; }
        public IBreakRepository BreakRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastAction"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public TimeSpan GetPauseInterval ( EmployeeAction lastAction, EmployeeAction action )
        {
            Period pause;

            if ( action.StartTime > lastAction.StartTime ) {
                pause = new Period( lastAction.EndTime(), action.StartTime );
            }
            else {
                pause = new Period( action.EndTime(), lastAction.StartTime );
            }

            if ( pause.Duration == TimeSpan.Zero || pause.Duration >= MinRestBetweenShifts ) return TimeSpan.Zero;


            TimeSpan resultPouse = TimeSpan.Zero;

            var longBreakDuration = BreakRepository.GetLongest( pause );
            var shortBreakDuration = BreakRepository.GetShortest( action.Employee );

            if ( longBreakDuration > TimeSpan.Zero 
                 && !_breaks.Contains( pause.Start.Date ) 
                 && pause.Duration >= longBreakDuration ) {

                // Check both intervals on fixed breaks, [Start + Longest : End] and [Start : End - Longest]

                var pauseAfter = BreakRepository.CheckFixed( 
                    new Period( pause.Start.Add( longBreakDuration ), pause.End ), action.Employee 
                );

                var pauseBefore = BreakRepository.CheckFixed(
                    new Period( pause.Start, pause.End.Subtract( longBreakDuration ) ), action.Employee
                );

                resultPouse = pauseAfter < pauseBefore
                                       ? (pause.Duration - longBreakDuration) + pauseAfter
                                       : (pause.Duration - longBreakDuration) + pauseBefore;
            }
            else if ( shortBreakDuration > TimeSpan.Zero
                      && pause.Duration >= shortBreakDuration ) {

                resultPouse = BreakRepository.CheckFixed( pause, action.Employee );
            }

            return resultPouse;
        }

    }
}
