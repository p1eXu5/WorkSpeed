using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Models.Productivity
{
    public interface IEmployeeProductivity
    {
        double GetTotalHours ();
        IEnumerable< (double count, Operation operation)> GetTimes ( IEnumerable< Operation > operations );
        IProductivity this[ Operation operation ] { get; }
        Employee Employee { get; }
    }
}
