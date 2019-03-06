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
            : base( operation, categories )
        {
            SpeedLabeling = SPEED_IN_SCANS;
            Speed = productivity.GetScansPerHour();
            SpeedTip = "Скорость сканирования";

            var scans = productivity.GetScans( _categories )
                                    .Select( t => (Convert.ToDouble( t.scans ), $"{t.category.Name}: {t.scans:F1} сканов") ).ToArray();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( scans ),
                Annotation = "сканн",
                Indicator = scans.Sum( t => t.Item1 ),
                IndicatorTip = "Сканов всего"
            } );


            var lines = productivity.GetLines( _categories )
                                    .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count:F1} строк") ).ToArray();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( lines ),
                Annotation = "строк",
                Indicator = lines.Sum( t => t.Item1 ),
                IndicatorTip = "Строк всего"
            } );


            var volumes = productivity.GetVolumes( _categories )
                                      .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count:F1} л.") ).ToArray();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( volumes ),
                Annotation = "кубов",
                Indicator = volumes.Sum( t => t.Item1 ) / 1000,
                IndicatorTip = "Объём всего (м3)"
            } );


            var quantities = productivity.GetQuantities( _categories )
                                         .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count:F1} шт.") ).ToArray();

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( quantities ),
                Annotation = "штук",
                Indicator = quantities.Sum( t => t.Item1 ),
                IndicatorTip = "Штук всего"
            } );

            Next( null );
        }
    }
}
