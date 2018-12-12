using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class GatheringIndicators : QuantityIndicators, IQuantityIndicators
    {
        public int ProductQuantity { get; private set; }
        public int LineQuantity { get; private set; }
        public double Volume { get; private set; }
        public double Weight { get; private set; }

        void IQuantityIndicators.AddQuantity ( EmployeeAction employeeAction )
        {
            if ( employeeAction is GatheringAction gatheringAction ) {

                ProductQuantity += gatheringAction.ProductQuantity;
                Volume += gatheringAction.Volume;
                Weight += gatheringAction.Weight;
                ++LineQuantity;
            }
        }

        public override string GetName ()
        {
            throw new NotImplementedException();
        }
    }
}
