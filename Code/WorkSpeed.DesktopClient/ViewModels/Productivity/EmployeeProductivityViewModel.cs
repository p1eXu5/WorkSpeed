using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.DesktopClient.ViewModels.Entities;

namespace WorkSpeed.DesktopClient.ViewModels.Productivity
{
    public class EmployeeProductivityViewModel : ViewModel
    {
        public EmployeeProductivityViewModel ( EmployeeProductivityBase employeeProductivity, Operation[] operations, List< Category > categories )
        {
            // Concrete productivities plus TimeProductivityViewModel
            ProductivityVms = new List< ProductivityViewModel >( operations.Length + 1 );

            EmployeeVm = new EmployeeViewModel( employeeProductivity.Employee );

            foreach ( var operation in operations ) {

                switch ( operation.Group ) {
                    case OperationGroups.Gathering:
                        ProductivityVms.Add( new GatheringProductivityViewModel( employeeProductivity[ operation ], categories ) );
                        break;
                    case OperationGroups.Reception:
                        ProductivityVms.Add( new ReceptionProductivityViewModel( employeeProductivity[ operation ], categories ) );
                        break;
                }
            }
        }

        public EmployeeViewModel EmployeeVm { get; }
        public List< ProductivityViewModel > ProductivityVms { get; }
    }
}
