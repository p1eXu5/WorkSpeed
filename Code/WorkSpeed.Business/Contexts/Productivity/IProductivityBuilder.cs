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

        OperationThresholds Thresholds { set; }

        ShortBreakInspectorMomento BuildNew ();

        (Period, EmployeeActionBase) CheckDuration ( EmployeeActionBase action, ShortBreakInspectorMomento momento );

        (Period, EmployeeActionBase) CheckPause ( (Period period, EmployeeActionBase action) currentAction, 
                                                  (Period period, EmployeeActionBase action ) nextAction,
                                                  ShortBreakInspectorMomento momento);

        void SubstractBreaks ( ShortBreakSchedule shortBreaks, ShortBreakInspectorMomento momento );
        void SubstractLunch ( Shift shift, ShortBreakInspectorMomento momento );
    }
}
