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
            if (employeeActions.Any()) return productivities.Values;

            Productivity productivity = null;
            Dictionary<Employee, EmployeeAction> prevOperations = new Dictionary<Employee, EmployeeAction>();

            foreach (var employeeAction in employeeActions) {

            }

            return null;
        }

        private static Productivity GetProductivity (ICollection<Productivity> productivities, EmployeeAction employeeActions)
        {
            var productivity = productivities.FirstOrDefault (p => p.Employee.Equals (employeeActions.Employee));

            if (null == productivity) {

            }

            return productivity;
        }

        public void Calculate ( SortedSet< EmployeeAction > actions, ICollection< Productivity > productivities )
        {
            throw new NotImplementedException();
        }
    }
}
