using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Entities;
using WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
{
    public class EmployeeProductivityViewModel : FilteredViewModel
    {
        #region Fields

        private readonly ReadOnlyObservableCollection< FilterViewModel > _filterVmCollection;
        private readonly ProductivityViewModel[] _productivityVmCollection;
        private IEnumerable< ProductivityViewModel > _filteredProductivity;

        #endregion


        #region Ctor

        public EmployeeProductivityViewModel ( IEmployeeProductivity employeeProductivity, 
                                               ReadOnlyObservableCollection< FilterViewModel > filters, 
                                               ReadOnlyObservableCollection< Category > categories
        ) {
            EmployeeProductivity = employeeProductivity ?? throw new ArgumentNullException(nameof(EmployeeProductivity), @"EmployeeProductivity cannot be null.");
            _filterVmCollection = filters ?? throw new ArgumentNullException(nameof(filters), @"filters cannot be null.");

            EmployeeVm = new EmployeeViewModel( employeeProductivity.Employee );

            _productivityVmCollection = CreateProductivityVmCollection();
            ProductivityVmCollection = _productivityVmCollection.Where( PredicateFunc ).OrderBy( o => o.OperationId ).ToArray();

            ProductivityViewModel[] CreateProductivityVmCollection ()
            {
                var operations = _filterVmCollection[ ( int )FilterIndexes.Operation ].FilterItemVmCollection.Select( fi => ( Operation )fi.Entity ).ToArray();

                // Concrete productivities plus TimeProductivityViewModel
                var vmColl = new List< ProductivityViewModel >( operations.Length + 1 );

                foreach ( var operation in operations ) {

                    switch ( operation.Group ) {
                        case OperationGroups.Reception :
                            vmColl.Add( new ReceptionProductivityViewModel( employeeProductivity[ operation ], operation, categories ) );
                            break;
                        case OperationGroups.Shipment :
                            vmColl.Add( new ShipmentProductivityViewModel( employeeProductivity[ operation ], operation ) );
                            break;
                        case OperationGroups.Other :
                            vmColl.Add( new OtherProductivityViewModel( employeeProductivity[ operation ], operation ) );
                            break;
                        case OperationGroups.Time:
                            vmColl.Add( new TimeProductivityViewModel( employeeProductivity, operations ) );
                            break;
                        default:
                            vmColl.Add( new GatheringProductivityViewModel( employeeProductivity[ operation ], operation, categories ) );
                            break;
                    }
                }

                return vmColl.ToArray();
            }
        }

        #endregion


        #region Properties

        public int? PositionId => EmployeeVm.Employee.PositionId;
        public int? AppointmentId => EmployeeVm.Employee.Appointment.Id;
        public string Name => EmployeeVm.Employee.Name;

        public IEmployeeProductivity EmployeeProductivity { get; }

        public EmployeeViewModel EmployeeVm { get; }

        public IEnumerable< ProductivityViewModel > ProductivityVmCollection
        {
            get => _filteredProductivity;
            private set {
                _filteredProductivity = value;
                OnPropertyChanged();
            }
        }

        public string[] Foo { get; set; }

        #endregion


        #region methods

        protected bool PredicateFunc ( ProductivityViewModel productivity )
        {
            return _filterVmCollection[ (int)FilterIndexes.Operation ].Entities.Any( obj => (obj as Operation) == productivity.Operation);
        }

        protected internal override void Refresh ( FilterIndexes filter )
        {
            // parallel processing slows down filtering
            ProductivityVmCollection = _productivityVmCollection.Where( PredicateFunc ).OrderBy( o => o.OperationId ).ToArray();
        }

        #endregion
    }
}
