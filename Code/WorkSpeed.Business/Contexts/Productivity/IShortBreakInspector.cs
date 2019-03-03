using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IShortBreakInspector
    {
        ShortBreakInspectorMomento SetBreak ( Period period );

        /// <summary>
        ///     Checks intersaction, move break to the next period.
        /// </summary>
        /// <param name="downtime"></param>
        /// <param name="momento"></param>
        /// <returns></returns>
        bool IsBreak ( Period downtime, ShortBreakInspectorMomento momento );
        ICollection< (TimeSpan start, TimeSpan end) > Breaks { get; }
        TimeSpan BreakDuration { get; }
        TimeSpan FirstBreakTime { get; }
        Period GetPreviousBreak ( ShortBreakInspectorMomento momento );
    }
}
