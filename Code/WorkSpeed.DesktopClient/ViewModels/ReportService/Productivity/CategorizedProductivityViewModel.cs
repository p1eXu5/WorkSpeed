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
    public class CategorizedProductivityViewModel : ProductivityViewModel
    {
        protected readonly Category[] _categories;
        protected readonly IProductivity _productivity;

        public CategorizedProductivityViewModel ( IProductivity productivity, Operation operation, IEnumerable< Category > categories ) : base( operation )
        {
            _categories = categories?.ToArray() ?? throw new ArgumentNullException(nameof(categories), @"categories cannot be null."); ;
            _productivity = productivity ?? throw new ArgumentNullException(nameof(productivity), @"IProductivity cannot be null.");
        }

        protected void AddLines ()
        {
             var lines = _productivity.GetLines( _categories )
                                    .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count:F1} строк") ).ToArray();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( lines ),
                Annotation = "строк",
                Indicator = lines.Sum( t => t.Item1 ),
                IndicatorTip = "Строк всего"
            } );
        }
    }
}
