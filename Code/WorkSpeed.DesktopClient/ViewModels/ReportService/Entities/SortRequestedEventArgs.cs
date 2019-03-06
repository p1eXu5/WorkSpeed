using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Entities
{
    public class SortRequestedEventArgs : EventArgs
    {
        public SortRequestedEventArgs ( SortOrder sortOrder )
        {
            SortOrder = sortOrder;
        }

        public SortOrder SortOrder { get; }
    }
}
