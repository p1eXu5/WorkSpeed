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


            var volumes = productivity.GetVolumes( _categories )
                                      .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count:F1} л.") ).ToArray();
            var volumSum = volumes.Sum( t => t.Item1 );
            var unitAnn = "л.";
            var ann = "литров";
            if ( volumSum > 100 ) {
                volumSum /= 1000;
                unitAnn = "м.";
                ann = "кубов";
            } 

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( volumes ),
                Annotation = ann,
                Indicator = volumSum,
                IndicatorTip = $"Объём всего ({unitAnn})"
            } );


            var quantities = productivity.GetQuantities( _categories )
                                         .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count:F1} шт.") ).ToArray();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( quantities ),
                Annotation = "штук",
                Indicator = quantities.Sum( t => t.Item1 ),
                IndicatorTip = "Штук всего"
            } );

            Next ( null );
        }

        #endregion
    }
}
