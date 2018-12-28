using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public interface IPauseBetweenActions
    {
        IBreakRepository BreakRepository { get; }
        TimeSpan GetPauseInterval ( EmployeeAction action, EmployeeAction lastAction );

        TimeSpan MinRestBetweenShifts { get; }
    }
}
