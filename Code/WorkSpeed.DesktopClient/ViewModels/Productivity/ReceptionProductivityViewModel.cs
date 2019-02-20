using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Productivity
{
    public class ReceptionProductivityViewModel : CategorizedProductivityViewModel
    {
        public ReceptionProductivityViewModel ( IProductivity productivity, IEnumerable< Category > categories ) 
            : base( productivity.Operation, categories )
        {
            SpeedLabeling = "cкан./ч.";
            Speed = productivity.GetScansPerHour();
            Aspects = productivity.GetScans( _categories )
                                  .Select( t => (Convert.ToDouble(t.Item1), $"{t.Item2.Name}: {t.Item1}") )
                                  .ToArray();
            Annotation = "сканов";
        }
    }
}
