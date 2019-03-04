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
            : base( new Operation { Id = -1 } )
        {
            SpeedLabeling = SPEED_IN_TIME;
            Speed = productivity.GetTotalWorkHours();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( productivity.GetOperationTimes( operations )
                                                                                    .Select( t => (Convert.ToDouble( t.hours ), $"{t.operation.Name}: {t.hours}") ) ),
                Annotation = "время"
            } );

            Next();
        }
    }
}
