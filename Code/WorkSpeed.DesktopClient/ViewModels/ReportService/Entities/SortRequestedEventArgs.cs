using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class SortRequestedEventArgs : EventArgs
    {
        public SortRequestedEventArgs ( Operation operation )
        {
            Operation = operation;
        }

        public Operation Operation { get; }
    }
}
