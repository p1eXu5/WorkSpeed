using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Contracts;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    /// <summary>
    ///     Base class for view models that have ability for contained collections filtering.
    ///     (Implements <see cref="IFilteredViewModel"/>)
    /// </summary>
    public abstract class FilteredViewModel : ViewModel, IFilteredViewModel
    {
        public List< ICollectionView > ViewList { get; protected set; }
        
        protected static Predicate< object > DefaultPredicate { get; } = o => !(( IFilteredViewModel )o).ViewList.All( v => v.IsEmpty );

        /// <summary>
        ///     Refrashes default collection views stored in ViewList.
        ///     (<see cref="IFilteredViewModel.Refresh"/>).
        /// </summary>
        public void Refresh ()
        {
            OnRefresh();
        }

        /// <summary>
        ///     Gets default view from source.
        ///     Assigns Predicate to ViewList.Filter if source can be filtered.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="predicate">When omited <see cref="DefaultPredicate"/> is using.</param>
        protected ICollectionView SetupView ( object source, Predicate< object > predicate = null )
        {
            var view = CollectionViewSource.GetDefaultView( source );

            if ( view.CanFilter ) {
                view.Filter = predicate ?? DefaultPredicate;
            }

            ViewList.Add( view );
            return view;
        }

        /// <summary>
        ///     Invokes by <see cref="Refresh"/>.
        /// </summary>
        protected virtual void OnRefresh ()
        {
            ViewList.ForEach( v => v.Refresh() );
        }
    }
}
