using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IProductivityBuilder
    {
        (IReadOnlyDictionary< Operation, IProductivity > productivityMap, HashSet< Period > downtimes) GetResult ();

        OperationThresholds Thresholds { set; }

        void BuildNew ();

        (Period, EmployeeActionBase) CheckDuration ( EmployeeActionBase action );

        (Period, EmployeeActionBase) CheckPause ( (Period period, EmployeeActionBase action) currentAction, 
                                                  (Period period, EmployeeActionBase action ) nextAction );

        void SubstractBreaks ( ShortBreakSchedule breaks );
        void SubstractLunch ( Shift shift );
    }
}
