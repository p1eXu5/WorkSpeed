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
        TimeSpan GetLongest ( Period period );
        TimeSpan GetShortest ( Employee employee );
        TimeSpan CheckFixed ( Period period, Employee employee );

        void SetVariableBreak ( string name, TimeSpan breakDuration, DayPeriod dayPeriod );

        void SetFixedBreaks ( string name,
                              TimeSpan duration,
                              TimeSpan interval,
                              TimeSpan offset,
                              Predicate< Employee > predicate );
    }
}
