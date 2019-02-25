using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Productivity
{
    public class GatheringProductivityViewModel : CategorizedProductivityViewModel
    {
        #region Ctor

        public GatheringProductivityViewModel ( IProductivity productivity, Operation operation, IEnumerable< Category > categories ) 
            : base( operation, categories )
        {
            SpeedLabeling = SPEED_IN_LINES;
            Speed = productivity.GetLinesPerHour();

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

            Next ();
        }

        #endregion
    }
}
