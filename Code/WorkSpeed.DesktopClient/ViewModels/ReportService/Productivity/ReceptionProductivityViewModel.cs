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
    public class ReceptionProductivityViewModel : CategorizedProductivityViewModel
    {
        public ReceptionProductivityViewModel ( IProductivity productivity, Operation operation, IEnumerable< Category > categories ) 
            : base( productivity, operation, categories )
        {
            SpeedLabeling = SPEED_IN_SCANS;
            Speed = productivity.GetScansPerHour();
            SpeedTip = "Скорость сканирования";

            AddScans();
            AddLines();
            AddVolumes();
            AddQuantities();

            NextSelectedAspect( null );
        }
    }
}
