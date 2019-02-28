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
            _queue.Enqueue( new AspectsViewModel {
                Aspects = new ObservableCollection< (double, string) >(),
            } );

            Next();
        }
    }
}
