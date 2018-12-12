using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class QuantityIndicators : ProductivityIndicators, IQuantityIndicators
    {
        private readonly ICategoryConstraints constraints;

        public QuantityIndicators ( string name, ICategoryConstraints constraints )
            : base(name)
        {

        }

        public ICategoryConstraints CategoryConstraints { get; set; }

        void IQuantityIndicators.AddQuantity ( EmployeeAction employeeAction )
        {
        }

        public static QuantityIndicators operator + ( QuantityIndicators indicators, EmployeeAction employeeAction )
        {
            (( IQuantityIndicators )indicators).AddQuantity( employeeAction );
            return indicators;
        }
    }
}
