﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Grouping
{
    public class PositionGroupingViewModel : FilteredViewModel
    {
        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        public PositionGroupingViewModel ( PositionGrouping positionGrouping, ReadOnlyObservableCollection< FilterViewModel > filters )
        {
            Position = positionGrouping.Position ?? throw new ArgumentNullException(nameof(positionGrouping), @"PositionGrouping cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");

            CreateCollection();

            var view = SetupView( EmployeeVmCollection );
            view.SortDescriptions.Add( new SortDescription( "IsNotActive", ListSortDirection.Ascending ) );
            view.SortDescriptions.Add( new SortDescription( "SecondName", ListSortDirection.Ascending ) );


            void CreateCollection ()
            {
                var employeeVmCollection = new ObservableCollection< EmployeeViewModel >( positionGrouping.Employees.Select( e =>
                                                                                                                             {
                                                                                                                                 var evm = new EmployeeViewModel( e );
                                                                                                                                 evm.PropertyChanged += OnIsModifyChanged;
                                                                                                                                 return evm;
                                                                                                                             } ) );
                EmployeeVmCollection = new ReadOnlyObservableCollection< EmployeeViewModel >( employeeVmCollection );
            }
        }

        public Position Position { get; }
        public ReadOnlyObservableCollection< EmployeeViewModel > EmployeeVmCollection { get; private set; }

         


        protected override void OnIsModifyChanged ( object sender, PropertyChangedEventArgs args )
        {
            base.OnIsModifyChanged( sender, args );

            IsModify = EmployeeVmCollection.Any( evm => evm.IsModify );
        }

        protected override bool PredicateFunc ( object o )
        {
            if (!(o is EmployeeViewModel employee)) return false;

            var res = _filterVmCollection[ ( int )Filters.IsActive ].Entities.Any( obj => ( bool )(obj).Equals( employee.IsActive ) )
                      && _filterVmCollection[ ( int )Filters.IsSmoker ].Entities.Any( obj => ( bool )(obj).Equals( employee.IsSmoker ) )
                      && _filterVmCollection[ ( int )Filters.Rank ].Entities.Any( obj => (obj as RankViewModel)?.Number == employee.Rank.Number );

            return res;
        }
    }
}
