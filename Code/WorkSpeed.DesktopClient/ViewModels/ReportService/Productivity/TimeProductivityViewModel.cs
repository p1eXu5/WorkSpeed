using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
{
    public class TimeProductivityViewModel : ProductivityViewModel
    {
        public TimeProductivityViewModel ( IEmployeeProductivity productivity, IEnumerable< Operation > operations )
            : base( operations.First( o => o.Id == -1 ) )
        {
            SpeedLabeling = SPEED_IN_TIME;
            var dt = productivity.GetTotalDowntimeHours();
            var wt = productivity.GetTotalWorkHours();
            Speed = wt + dt;
            SpeedTip = "Общее время работы (с простоями)";


            List< (double,string) > timeList = new List< (double, string) >( 
                productivity.GetOperationTimes( operations.Where( o => o.Id > 0 ) )
                            .Where( t => t.hours > 0.0 )
                            .Select( t => (Convert.ToDouble( t.hours ), $"{t.operation.Name}: {t.hours:F1}") ) 
            );

            if ( dt > 0.0 ) {
                timeList.Add( (Convert.ToDouble( dt ), $"Время бездействия: {dt:F1}") );
            }

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( timeList ),
                Indicator = wt,
                IndicatorTip = "Время работы",
                Annotation = "время"
            } );

            Next( null );
        }
    }
}
