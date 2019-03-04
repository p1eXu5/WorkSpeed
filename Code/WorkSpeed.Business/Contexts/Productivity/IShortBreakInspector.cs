using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IShortBreakInspector
    {
        ICollection< (TimeSpan start, TimeSpan end) > Breaks { get; }

        ShortBreakInspectorMomento SetBreak ( Period period );

        /// <summary>
        ///     Checks intersaction, move break to the next period.
        /// </summary>
        /// <param name="downtime"></param>
        /// <param name="momento"></param>
        /// <returns></returns>
        bool IsBreak ( Period downtime, ref ShortBreakInspectorMomento momento );
        Period GetPreviousBreak ( ref ShortBreakInspectorMomento momento );
        TimeSpan BreakDuration { get; }
        TimeSpan FirstBreakTime { get; }
    }
}
