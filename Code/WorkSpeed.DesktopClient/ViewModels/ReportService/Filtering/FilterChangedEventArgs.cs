using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering
{
    public class FilterChangedEventArgs : EventArgs
    {
        public FilterChangedEventArgs ( FilterIndexes filterIndex )
        {
            FilterIndex = filterIndex;
        }

        public FilterIndexes FilterIndex { get; }
    }
}
