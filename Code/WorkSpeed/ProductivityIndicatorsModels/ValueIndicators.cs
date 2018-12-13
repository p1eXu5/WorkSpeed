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
    public class ValueIndicators : QuantityIndicators
    {
        protected List< double > _valueList;

        public ValueIndicators ( string name ) : base( name )
        {
            _valueList = new List< double >( 1 );
        }

        public ValueIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints )
        {
            _valueList = new List< double >( _categoryConstraints.Count );
        }

        protected override void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints )
        {
            throw new NotImplementedException();
        }

        protected override void Add ( EmployeeAction employeeAction )
        {
            throw new NotImplementedException();
        }
    }
}
