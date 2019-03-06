using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
{
    public class OtherProductivityViewModel : ProductivityViewModel
    {
        public OtherProductivityViewModel ( IProductivity productivity, Operation operation )
            : base( operation )
        {
            SpeedLabeling = SPEED_IN_TIME;
            var dt = productivity.GetTotalHours();
            Speed = dt;
            SpeedTip = "Время остальных операций";


            _queue.Enqueue( new AspectsViewModel {
                Aspects = new ObservableCollection< (double, string) >(),
                Annotation = "время",
                Indicator = dt,
                IndicatorTip = "Время остальных операций",
            } );

            NextSelectedAspect( null );
        }
    }
}
