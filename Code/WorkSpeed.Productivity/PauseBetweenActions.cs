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
        private readonly Dictionary< DateTime, Queue< Shift >> _catchedLunches;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="breakRepository"></param>
        /// <param name="minRestBetweenShifts">Minimum pause between shifts.</param>
        public PauseBetweenActions ( IBreakRepository breakRepository)
        {
            BreakRepository = breakRepository ?? throw new ArgumentNullException( nameof( breakRepository ), "IBreakRepository cannot be null." );

            _catchedLunches = new Dictionary<DateTime, Queue<Shift>> ();
        }

        public TimeSpan MinRestBetweenShifts { get; private set; } = TimeSpan.FromHours( 5 );

        public IBreakRepository BreakRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastAction"></param>
        /// <param name="action"></param>
        /// <returns>TimeSpan.Zero if another shift.</returns>
        public TimeSpan GetPauseInterval ( EmployeeAction lastAction, EmployeeAction action )
        {
            Period pause;

            if ( action.StartTime > lastAction.StartTime ) {
                pause = new Period( lastAction.EndTime(), action.StartTime );
            }
            else {
                pause = new Period( action.EndTime(), lastAction.StartTime );
            }

            var duration = pause.Duration;

            if ( duration <= TimeSpan.Zero ) throw new InvalidOperationException("Pause duration less than TimeSpan.Zero");

            if ( duration > MinRestBetweenShifts ) return TimeSpan.Zero;

            if ( duration < BreakRepository.ShortBreakDownLimit ) return pause.Duration;

            if ( duration > BreakRepository.ShortBreakUpLimit ) {

                var shiftList = BreakRepository.CheckLunchBreak( pause );

                if ( shiftList.Any() ) {

                    for ( int i = 0; i < shiftList.Length; ++i ) {

                        var date = lastAction.StartTime.Date;

                        if ( !_catchedLunches.ContainsKey( date )
                             || !_catchedLunches[ date ].Contains( shiftList[ i ] )) {

                            if ( duration > shiftList[ i ].LunchDuration ) {

                                duration -= shiftList[ i ].LunchDuration;

                                if ( !_catchedLunches.ContainsKey( date ) ) {
                                    _catchedLunches[ date ] = new Queue< Shift >( new [] { shiftList[ i ] } );
                                }
                                else {
                                    _catchedLunches[ date ].Enqueue( shiftList[ i ] );
                                }
                            }
                        }
                    }
                }
            }

            if ( duration >= BreakRepository.ShortBreakDownLimit ) {

                if ( pause.Duration > duration ) {

                    var leftPause = new Period( pause.Start, pause.End.Subtract( duration ) );
                    var rightPause = new Period( pause.Start.Add( duration ), pause.End );

                    var leftShortBreakRes = BreakRepository.CheckShortBreak( leftPause, lastAction.Employee );
                    var rightShortBreakRes = BreakRepository.CheckShortBreak( rightPause, lastAction.Employee );

                    (ShortBreak shortBreak, TimeSpan breakLength) resShortBreakRes = (null, TimeSpan.Zero);

                    if ( leftShortBreakRes.shortBreak != null && rightShortBreakRes.shortBreak != null ) {

                        if ( leftShortBreakRes.breakLength > rightShortBreakRes.breakLength ) {
                            resShortBreakRes = leftShortBreakRes;
                        }
                        else {
                            resShortBreakRes = rightShortBreakRes;
                        }
                    }
                    else if ( rightShortBreakRes.shortBreak != null ) {
                        resShortBreakRes = rightShortBreakRes;
                    }
                    else if ( leftShortBreakRes.shortBreak != null ) {
                        resShortBreakRes = leftShortBreakRes;
                    }

                    duration -= resShortBreakRes.breakLength;

                    while ( duration < TimeSpan.Zero ) {
                        duration += leftShortBreakRes.shortBreak.Duration;
                    }
                }
                else {

                    var shortBreakRes = BreakRepository.CheckShortBreak( pause, lastAction.Employee );

                    if ( shortBreakRes.shortBreak != null ) {

                        duration -= shortBreakRes.breakLength;

                        while ( duration < TimeSpan.Zero ) {
                            duration += shortBreakRes.shortBreak.Duration;
                        }
                    }
                }
            }

            return duration;
        }

    }
}
