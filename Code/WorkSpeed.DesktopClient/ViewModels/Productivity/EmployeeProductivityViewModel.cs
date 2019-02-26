using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.DesktopClient.ViewModels.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService;

namespace WorkSpeed.DesktopClient.ViewModels.Productivity
{
    public class EmployeeProductivityViewModel : FilteredViewModel
    {
        #region Ctor

        public EmployeeProductivityViewModel ( IEmployeeProductivity employeeProductivity, 
                                               ReadOnlyObservableCollection< Operation > operations, 
                                               ReadOnlyObservableCollection< Category > categories,
                                               Predicate< object > predicate
        ) {
            EmployeeProductivity = employeeProductivity ?? throw new ArgumentNullException(nameof(EmployeeProductivity), @"EmployeeProductivity cannot be null.");
            EmployeeVm = new EmployeeViewModel( employeeProductivity.Employee );

            CreateProductivityVmCollection( employeeProductivity, operations, categories );

            var view = SetupView( ProductivityVmCollection, predicate );
            view.SortDescriptions.Add( new SortDescription( "OperationId", ListSortDirection.Ascending ) );
        }

        private void CreateProductivityVmCollection ( IEmployeeProductivity employeeProductivity, 
                                                      ReadOnlyObservableCollection< Operation > operations, 
                                                      ReadOnlyObservableCollection< Category > categories )
        {
            // Concrete productivities plus TimeProductivityViewModel
            ProductivityVmCollection = new List< ProductivityViewModel >( operations.Count + 1 );

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

        #endregion

        public IEmployeeProductivity EmployeeProductivity { get; }

        public EmployeeViewModel EmployeeVm { get; }
        public List< ProductivityViewModel > ProductivityVmCollection { get; private set; }
    }
}
