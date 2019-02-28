using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
{
    public class EmployeeProductivityViewModel : FilteredViewModel
    {
        #region Fields

        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;

        #endregion


        #region Ctor

        public EmployeeProductivityViewModel ( IEmployeeProductivity employeeProductivity, 
                                               ReadOnlyObservableCollection< FilterViewModel > filters, 
                                               ReadOnlyObservableCollection< Category > categories
        ) {
            EmployeeProductivity = employeeProductivity ?? throw new ArgumentNullException(nameof(EmployeeProductivity), @"EmployeeProductivity cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");

            EmployeeVm = new EmployeeViewModel( employeeProductivity.Employee );

            CreateProductivityVmCollection();

            var view = SetupView( ProductivityVmCollection );
            view.SortDescriptions.Add( new SortDescription( "OperationId", ListSortDirection.Ascending ) );



            void CreateProductivityVmCollection ()
            {
                var operations = _filterVmCollection[ ( int )FilterIndexes.Operation ].FilterItemVmCollection.Select( fi => ( Operation )fi.Entity ).ToArray();

                // Concrete productivities plus TimeProductivityViewModel
                ProductivityVmCollection = new List< ProductivityViewModel >( operations.Length + 1 );

                foreach ( var operation in operations ) {
                    switch ( operation.Group ) {
                        case OperationGroups.Gathering :
                        case OperationGroups.Packing :
                        case OperationGroups.Placing :
                        case OperationGroups.BuyerGathering :
                        case OperationGroups.Defragmentation :
                        case OperationGroups.Inventory :
                            ProductivityVmCollection.Add( new GatheringProductivityViewModel( employeeProductivity[ operation ], operation, categories ) );
                            break;
                        case OperationGroups.Reception :
                            ProductivityVmCollection.Add( new ReceptionProductivityViewModel( employeeProductivity[ operation ], operation, categories ) );
                            break;
                        case OperationGroups.Shipment :
                            ProductivityVmCollection.Add( new ShipmentProductivityViewModel( employeeProductivity[ operation ], operation ) );
                            break;
                        case OperationGroups.Other :
                            ProductivityVmCollection.Add( new OtherProductivityViewModel( employeeProductivity[ operation ], operation ) );
                            break;
                    }
                }

                ProductivityVmCollection.Add( new TimeProductivityViewModel( employeeProductivity, operations ) );
            }
        }

        #endregion


        #region Properties

        public IEmployeeProductivity EmployeeProductivity { get; }

        public EmployeeViewModel EmployeeVm { get; }
        public List< ProductivityViewModel > ProductivityVmCollection { get; private set; }

        #endregion


        #region methods

        protected override bool PredicateFunc ( object o )
        {
            if ( !(o is ProductivityViewModel productivity) ) return false;

            var res = _filterVmCollection[ (int)FilterIndexes.Operation ].Entities.Any( obj => (obj as Operation) == productivity.Operation);

            return res;
        }
        
        #endregion
    }
}
