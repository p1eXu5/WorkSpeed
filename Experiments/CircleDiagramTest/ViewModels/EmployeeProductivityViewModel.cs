using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class EmployeeProductivityViewModel : ViewModel
    {
        private readonly Employee _employee;

        public EmployeeProductivityViewModel ( EmployeeProductivity employeeProductivity, Operation[] operations, List< Category > categories )
        {
            // Concrete productivities plus TimeProductivityViewModel
            Productivities = new List< ProductivityViewModel >( operations.Length + 1 );

            _employee = employeeProductivity.Employee;

            foreach ( var operation in operations ) {

                switch ( operation.Group ) {
                    case OperationGroups.Gathering:
                        Productivities.Add( new GatheringProductivityViewModel( employeeProductivity[ operation ], categories ) );
                        break;
                    case OperationGroups.Reception:
                        Productivities.Add( new ReceptionProductivityViewModel( employeeProductivity[ operation ], categories ) );
                        break;
                }
            }
        }

        public List< ProductivityViewModel > Productivities { get; }
    }
}
