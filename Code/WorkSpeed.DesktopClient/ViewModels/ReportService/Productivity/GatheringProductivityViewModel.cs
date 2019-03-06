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
    public class GatheringProductivityViewModel : CategorizedProductivityViewModel
    {
        #region Ctor

        public GatheringProductivityViewModel ( IProductivity productivity, Operation operation, IEnumerable< Category > categories ) 
            : base( productivity, operation, categories )
        {
            SpeedLabeling = SPEED_IN_LINES;
            Speed = productivity.GetLinesPerHour();
            SpeedTip = "Скорость набора строк";

            AddLines();
            AddVolumes();
            AddQuantities();

            NextSelectedAspect ( null );
        }

        #endregion
    }
}
