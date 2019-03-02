using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public struct ShortBreakInspectorMomento
    {
        public Period Break { get; internal set; }

        public ShortBreakInspectorMomento ( Period @break )
        {
            Break = @break;
        }
    }
}
