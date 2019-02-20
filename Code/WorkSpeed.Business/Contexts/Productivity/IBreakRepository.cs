using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IBreakRepository
    {

        TimeSpan ShortBreakDownLimit { get; }
        TimeSpan ShortBreakUpLimit { get; }

        IEnumerable<Shift> ShiftCollection { get; }
        IEnumerable<ShortBreakSchedule> ShortBreakCollection { get; }

        void AddFixedBreak ( ShortBreakSchedule shortBreak, Predicate< Employee > predicate );
        void AddVariableBreak ( Shift shift );
        (ShortBreakSchedule shortBreak, TimeSpan breakLength) CheckShortBreak ( Period period, Employee employee );
        Shift[] CheckLunchBreak ( Period period );
    }
}
