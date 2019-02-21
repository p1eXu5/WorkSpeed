using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity.Builders
{
    public interface IProductivityDirector
    {
        IEnumerable< IProductivity > GetProductivities ( IEnumerable< EmployeeActionBase > actions, OperationThresholds thresholds );
    }
}
