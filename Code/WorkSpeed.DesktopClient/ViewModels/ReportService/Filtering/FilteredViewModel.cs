using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Contracts;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering
{
    /// <summary>
    ///     Base class for view models that have ability for contained collections filtering.
    ///     (Implements <see cref="ICollectionViewList"/>)
    /// </summary>
    public abstract class FilteredViewModel : ViewModel, ICollectionViewList
    {
        private bool _isModify;

        protected FilteredViewModel ()
        {
            ViewList = new List< ListCollectionView >(2);
        }

        public List< ListCollectionView > ViewList { get; protected set; }
        

        public bool IsModify
        {
            get => _isModify;
            set {
                _isModify = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets default view from source.
        ///     Assigns Predicate to ViewList.Filter if source can be filtered.
        /// </summary>
        /// <param name="source">Source collection.</param>
        /// <param name="callback"></param>
        protected void SetupView ( object source, Action< ListCollectionView > callback = null )
        {
            ListCollectionView view = ( ListCollectionView )CollectionViewSource.GetDefaultView( source );

            if ( view.CanFilter ) {
                if ( callback == null ) {
                    view.Filter = PredicateFunc;
                }
                else {
                    callback( view );
                }
            }

            ViewList.Add( view );
        }

        protected internal virtual void Refresh ()
        {
            foreach ( var view in ViewList ) {
                    view.Refresh();
            }
        }

        /// <summary>
        ///     Checks is this IsModifyProperty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( nameof( IsModify ) ) ) return;
        }

        protected virtual bool PredicateFunc ( object o )
        {
            return (( ICollectionViewList )o).ViewList.All( v => !v.IsEmpty );
        }
    }
}
