using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class QuantityIndicators : ProductivityIndicators, IQuantityIndicators
    {
        protected ICategoryConstraints _categoryConstraints;

        public QuantityIndicators ( string name )
            : base( name )
        { }

        public QuantityIndicators ( string name, ICategoryConstraints constraints )
            : base( name )
        {
            _categoryConstraints = constraints ?? throw new ArgumentNullException();
        }

        public ICategoryConstraints CategoryConstraints
        {
            get => _categoryConstraints; 
            internal set => _categoryConstraints = value ?? throw new ArgumentNullException();
        }

        public void AddQuantity ( EmployeeAction employeeAction )
        {
            Add( employeeAction );
        }

        protected virtual void Add ( EmployeeAction employeeAction )
        {
            throw new NotImplementedException();
        }

        public static QuantityIndicators operator + ( QuantityIndicators indicators, EmployeeAction employeeAction )
        {
            (( IQuantityIndicators )indicators).AddQuantity( employeeAction );
            return indicators;
        }
    }
}
