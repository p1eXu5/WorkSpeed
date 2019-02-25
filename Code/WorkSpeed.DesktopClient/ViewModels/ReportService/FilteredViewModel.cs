using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public abstract class FilteredViewModel : ViewModel, IFilteredViewModel
    {
        public ICollectionView View { get; protected set; }
        
        public void Refresh ()
        {
            OnRefresh();
        }

        /// <summary>
        ///     Gets default view from source.
        ///     Assigns Predicate to View.Filter if source can be filtered.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        protected ICollectionView SetupView ( object source, Predicate< object > predicate = null )
        {
            var view = CollectionViewSource.GetDefaultView( source );

            if ( view.CanFilter ) {
                view.Filter = predicate ?? (o => !((IFilteredViewModel)o).View.IsEmpty);
            }

            return view;
        }

        protected virtual void OnRefresh ()
        {
            View.Refresh();
        }
    }
}
