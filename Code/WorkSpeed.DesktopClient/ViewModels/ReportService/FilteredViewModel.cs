﻿using System;
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
    ///     (Implements <see cref="ICollectionViewList"/>)
    /// </summary>
    public abstract class FilteredViewModel : ViewModel, ICollectionViewList
    {
        private bool _isModify;

        protected FilteredViewModel ()
        {
            ViewList = new List< ICollectionView >(2);
        }

        public List< ICollectionView > ViewList { get; protected set; }
        

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
        /// <param name="predicate"></param>
        protected ICollectionView SetupView ( object source, Predicate< object > predicate = null )
        {
            var view = CollectionViewSource.GetDefaultView( source );

            if ( view.CanFilter ) {
                view.Filter = predicate ?? PredicateFunc;
            }

            ViewList.Add( view );
            return view;
        }

        protected internal virtual void Refresh ()
        {
            foreach ( var view in ViewList ) {
                try {
                    view.Refresh();
                }
                catch ( Exception ex ) {
                    ;
                }
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
            return !(( ICollectionViewList )o).ViewList.All( v => v.IsEmpty );
        }
    }
}
