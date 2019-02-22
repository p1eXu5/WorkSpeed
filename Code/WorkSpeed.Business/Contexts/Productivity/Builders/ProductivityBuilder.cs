using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.Productivity.Builders
{
    public class ProductivityBuilder : IProductivityBuilder
    {
        private readonly Dictionary< Operation, IProductivity > _productivitys;

        public ProductivityBuilder ()
        {
            _productivitys = new Dictionary< Operation, IProductivity >();
        }

        public IReadOnlyDictionary< Operation, IProductivity > Productivities => _productivitys;
        public OperationThresholds Thresholds { get; set; }
        public void CheckDuration ( EmployeeActionBase action )
        {
            throw new NotImplementedException();
        }

        public void CheckPause ( EmployeeActionBase currentAction, EmployeeActionBase nextAction )
        {
            throw new NotImplementedException();
        }

        public void SubstractBreaks ( ShortBreakSchedule breaks )
        {
            throw new NotImplementedException();
        }

        public void SubstractLunch ( Shift shift )
        {
            throw new NotImplementedException();
        }
    }
}
