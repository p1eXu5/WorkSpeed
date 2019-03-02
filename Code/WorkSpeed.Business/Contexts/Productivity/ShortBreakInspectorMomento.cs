using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public struct ShortBreakInspectorMomento : IShortBreakInspectorMomento
    {
        public Period Break { get; private set; }

        public ShortBreakInspectorMomento ( Period @break )
        {
            Break = @break;
        }
    }
}
