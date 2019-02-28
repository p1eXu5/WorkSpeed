using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity.Builders
{
    public interface IProductivityCalculator
    {
        IEnumerable< Models.Productivity > Calculate ( IEnumerable< EmployeeActionBase > actions );
    }
}
