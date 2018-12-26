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

        public PauseBetweenActions ( IBreakRepository breakRepository )
        {
            _breakRepository = breakRepository ?? throw new ArgumentNullException( nameof( breakRepository ), "IBreakRepository cannot be null." );
        }

        public TimeSpan GetPauseInterval ( EmployeeAction lastAction, EmployeeAction action )
        {
            Period pause;

            if ( action.StartTime > lastAction.StartTime ) {
                pause = new Period( lastAction.EndTime(), action.StartTime );
            }
            else {
                pause = new Period( action.EndTime(), lastAction.StartTime );
            }

            if ( pause == Period.Zero ) return TimeSpan.Zero;

            throw new NotImplementedException();
        }
    }
}
