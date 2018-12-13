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
    public abstract class QuantityIndicators : ProductivityIndicators, IQuantityIndicators
    {
        protected ICategoryConstraints _categoryConstraints;

        protected QuantityIndicators ( string name )
            : base( name )
        { }

        protected QuantityIndicators ( string name, ICategoryConstraints constraints )
            : base( name )
        {
            _categoryConstraints = constraints ?? throw new ArgumentNullException();
        }

        public ICategoryConstraints CategoryConstraints
        {
            get => _categoryConstraints; 
            internal set => OnChangeCategoryConstraints( value );
        }

        public void AddQuantity ( EmployeeAction employeeAction )
        {
            Add( employeeAction );
        }

        protected abstract void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints );

        protected abstract void Add ( EmployeeAction employeeAction );

        public static QuantityIndicators operator + ( QuantityIndicators indicators, EmployeeAction employeeAction )
        {
            indicators.AddQuantity( employeeAction );
            return indicators;
        }
    }
}
