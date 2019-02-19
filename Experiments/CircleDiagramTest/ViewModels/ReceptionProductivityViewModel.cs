using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class ReceptionProductivityViewModel : CategorizedProductivityViewModel
    {
        public ReceptionProductivityViewModel ( IProductivity productivity, IEnumerable< Category > categories ) 
            : base( productivity.Operation, categories )
        {
            SpeedLabeling = "cкан./ч.";
            Speed = productivity.GetScansPerHour();
            Aspects = productivity.GetScans( _categories ).Select( Convert.ToDouble ).ToArray();
        }

        public IEnumerable< double > Aspects { get; set; }
    }
}
