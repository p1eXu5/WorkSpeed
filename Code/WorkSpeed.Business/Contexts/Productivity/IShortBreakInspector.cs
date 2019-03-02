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
        bool IsBreak ( Period downtime, ShortBreakInspectorMomento momento );
        ICollection< (TimeSpan start, TimeSpan end) > Breaks { get; }
    }
}
