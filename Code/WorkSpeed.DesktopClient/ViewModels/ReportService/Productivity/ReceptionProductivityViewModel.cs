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

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( productivity.GetScans( _categories )
                                                                                    .Select( t => (Convert.ToDouble( t.scans ), $"{t.category.Name}: {t.scans}") ) ),
                Annotation = "сканн"
            } );

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( productivity.GetLines( _categories )
                                                                                    .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count}") ) ),
                Annotation = "строк"
            } );

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( productivity.GetVolumes( _categories )
                                                                                    .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count}") ) ),
                Annotation = "кубов"
            } );

            _queue.Enqueue( new AspectsViewModel {

                Aspects = new ObservableCollection< (double, string) >( productivity.GetQuantity( _categories )
                                                                                    .Select( t => (Convert.ToDouble( t.count ), $"{t.category.Name}: {t.count}") ) ),
                Annotation = "штук"
            } );

            Next();
        }
    }
}
