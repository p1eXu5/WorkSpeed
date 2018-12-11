using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Comparers;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityCalculator
{
    class ProductivityCalculator : IProductivityCalculator<EmployeeAction>
    {
        public ICollection<Productivity> Calculate (SortedSet<EmployeeAction> actions)
        {
            if (actions == null) throw new ArgumentNullException();

            var employeeActions = actions.ToArray();

            var productivities = new Dictionary<Employee, Productivity>();
            if (employeeActions.Any()) return productivities;

            Productivity productivity = null;
            Dictionary<Employee, EmployeeAction> prevOperations = new Dictionary<Employee, EmployeeAction>();

            foreach (var employeeAction in employeeActions) {

                if (prevOperations.ContainsKey(employeeAction.Employee)) {

                    productivities[employeeAction.Employee] = new Productivity (employeeAction);
                    prevOperations[employeeAction.Employee] = employeeAction;
                    continue;
                }

                if (employeeAction.Operation.Equals (prevOperations[employeeAction.Employee].Operation)) {

                }
            }
        }

        private static Productivity GetProductivity (ICollection<Productivity> productivities, EmployeeAction employeeActions)
        {
            var productivity = productivities.FirstOrDefault (p => p.Employee.Equals (employeeActions.Employee));

            if (null == productivity) {
                productivity = new Productivity (employeeActions.Employee);
                productivities.Add (productivity);
            }

            return productivity;
        }
    }
}
