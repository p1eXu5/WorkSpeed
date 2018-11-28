using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Manager.Models;

namespace WorkSpeed.Manager
{
    public static class ProductivityCalculator
    {
        public static Productivity CalculateProductivity(this Employee employee, IEnumerable<TempAction> actions, WarehouseConstraints constraints)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee), "There is no employee!");
            if (actions == null) throw new ArgumentNullException(nameof(employee), "There are no actions!");

            var actionCollection = actions.Where (a => a.Employee.Equals(employee)).OrderBy (a => a.OperationStart).ToList();
            if (!actionCollection.Any()) return new Productivity();
            
            var operationsDetails = new Dictionary<Operation, OperationDetails>();

            var currentAction = actionCollection[0];

            int i;
            for (i = 0; i < actionCollection.Count - 1; ++i) {

                var action = actionCollection[i];

                if (!operationsDetails.ContainsKey (actionCollection[i].Operation)) {
                    operationsDetails[actionCollection[i].Operation] = new OperationDetails();
                }

                operationsDetails[currentAction.Operation].AddDurationTime
                (
                    constraints.GetDurationWithoutBreaks (actionCollection[i].Operation,
                                                            actionCollection[i + 1].Operation)
                );

                operationsDetails[currentAction.Operation].AddDetails(actionCollection[i].Operation, constraints.Categories);
            }

            operationsDetails[currentAction.Operation].AddDurationTime
            (
                constraints.GetDurationWithoutBreaks(actionCollection[i].Operation)
            );

            operationsDetails[currentAction.Operation].AddDetails(actionCollection[i].Operation, constraints.Categories);


            return new Productivity(operationsDetails);
        }
    }
}
