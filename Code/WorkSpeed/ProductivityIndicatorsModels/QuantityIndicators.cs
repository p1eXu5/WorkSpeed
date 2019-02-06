using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.ProductivityIndicatorsModels
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

        public void AddQuantity ( EmployeeActionBase employeeAction )
        {
            Add( employeeAction );
        }

        protected abstract void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints );

        public static QuantityIndicators operator + ( QuantityIndicators indicators, EmployeeActionBase employeeAction )
        {
            indicators.AddQuantity( employeeAction );
            return indicators;
        }
    }
}
