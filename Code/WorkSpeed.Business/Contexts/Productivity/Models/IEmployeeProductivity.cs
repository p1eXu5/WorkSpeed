using System.Collections.Generic;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity.Models
{
    public interface IEmployeeProductivity
    {
        double GetTotalWorkHours ();
        double GetTotalDowntimeHours ();
        IEnumerable< (double hours, Operation operation)> GetOperationTimes ( IEnumerable< Operation > operations );
        IProductivity this[ Operation operation ] { get; }
        Employee Employee { get; }
    }
}
