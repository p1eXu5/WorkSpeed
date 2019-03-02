using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IShortBreakInspectorFactory
    {
        IShortBreakInspector GetShortBreakInspector ( ShortBreakSchedule shortBreaks );
    }
}
