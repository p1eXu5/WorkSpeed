using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IPauseBetweenActions
    {
        IBreakRepository BreakRepository { get; }
        TimeSpan GetPauseInterval ( EmployeeActionBase action, EmployeeActionBase lastAction );

        TimeSpan MinRestBetweenShifts { get; }
    }
}
