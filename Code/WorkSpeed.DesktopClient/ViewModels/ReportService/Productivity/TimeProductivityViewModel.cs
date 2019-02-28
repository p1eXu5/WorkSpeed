using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
{
    public class TimeProductivityViewModel : ProductivityViewModel
    {
        public TimeProductivityViewModel ( IEmployeeProductivity productivity, ReadOnlyObservableCollection< Operation > operations )
            : base( new Operation { Id = -1 } )
        {
            SpeedLabeling = SPEED_IN_TIME;
            Speed = productivity.GetTotalHours();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( productivity.GetTimes( operations ).Select( t => (Convert.ToDouble( t.count ), $"{t.operation.Name}: {t.count}") ) ),
                Annotation = "время"
            } );

            Next();
        }
    }
}
