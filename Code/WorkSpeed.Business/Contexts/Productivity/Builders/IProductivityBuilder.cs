using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity.Builders
{
    public interface IProductivityBuilder
    {
        IReadOnlyDictionary< Operation, IProductivity > Productivities { get; }

        OperationThresholds Thresholds { set; }

        void BuildNew ();
        void CheckDuration ( EmployeeActionBase action );
        void CheckPause ( EmployeeActionBase currentAction, EmployeeActionBase nextAction );
        void SubstractBreaks ( ShortBreakSchedule breaks );
        void SubstractLunch ( Shift shift );
    }
}
