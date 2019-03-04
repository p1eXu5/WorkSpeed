using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity.Models
{
    public interface IEmployeeProductivity
    {
        double GetTotalWorkHours ();
        IEnumerable< (double hours, Operation operation)> GetOperationTimes ( IEnumerable< Operation > operations );
        IProductivity this[ Operation operation ] { get; }
        Employee Employee { get; }
    }
}
