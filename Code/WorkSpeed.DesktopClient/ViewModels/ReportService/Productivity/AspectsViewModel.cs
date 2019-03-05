using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
{
    public class AspectsViewModel
    {
        public ObservableCollection< (double,string) > Aspects { get; set; }
        public string Annotation { get; set; }
        public double Indicator { get; set; }
        public string IndicatorTip { get; set; }
    }
}
