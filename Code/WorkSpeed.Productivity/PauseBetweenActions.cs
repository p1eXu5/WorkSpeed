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
        private readonly IBreakRepository _breakRepository;
        private readonly TimeSpan _minBetweenShifts;
        private readonly HashSet< DateTime > _breaks;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="breakRepository"></param>
        /// <param name="minBetweenShifts">Minimum pause between shifts.</param>
        public PauseBetweenActions ( IBreakRepository breakRepository, TimeSpan minBetweenShifts )
        {
            _breakRepository = breakRepository ?? throw new ArgumentNullException( nameof( breakRepository ), "IBreakRepository cannot be null." );
            _minBetweenShifts = minBetweenShifts;
            _breaks = new HashSet< DateTime >();
        }

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

            if ( pause.Duration == TimeSpan.Zero || pause.Duration >= _minBetweenShifts ) return TimeSpan.Zero;


            TimeSpan resultPouse = TimeSpan.Zero;

            var longBreakDuration = _breakRepository.GetLongest( pause );
            var shortBreakDuration = _breakRepository.GetShortest( action.Employee );

            if ( !_breaks.Contains( pause.Start.Date ) && pause.Duration >= longBreakDuration ) {

                // Check both intervals on fixed breaks, [Start + Longest : End] and [Start : End - Longest]

                var pauseAfter = _breakRepository.CheckFixed( 
                    new Period( pause.Start.Add( longBreakDuration ), pause.End ), action.Employee 
                );

                var pauseBefore = _breakRepository.CheckFixed(
                    new Period( pause.Start, pause.End.Subtract( longBreakDuration ) ), action.Employee
                );

                resultPouse = pauseAfter < pauseBefore
                                       ? (pause.Duration - longBreakDuration) + pauseAfter
                                       : (pause.Duration - longBreakDuration) + pauseBefore;
            }
            else if ( pause.Duration >= shortBreakDuration ) {

                resultPouse = _breakRepository.CheckFixed( pause, action.Employee );
            }

            return resultPouse;
        }

    }
}
