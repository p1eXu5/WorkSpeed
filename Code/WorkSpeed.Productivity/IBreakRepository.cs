using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public interface IBreakRepository
    {

        TimeSpan ShortBreakDownLimit { get; }
        TimeSpan ShortBreakUpLimit { get; }

        IEnumerable<Shift> ShiftCollection { get; }
        IEnumerable<ShortBreak> ShortBreakCollection { get; }

        void AddFixedBreak ( ShortBreak shortBreak, Predicate< Employee > predicate );
        void AddVariableBreak ( Shift shift );
        (ShortBreak shortBreak, TimeSpan breakLength) CheckShortBreak ( Period period, Employee employee );
        Shift[] CheckLunchBreak ( Period period );
    }
}
