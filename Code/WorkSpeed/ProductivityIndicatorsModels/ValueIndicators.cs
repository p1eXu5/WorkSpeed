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
    public abstract class ValueIndicators : QuantityIndicators
    {
        protected List< double > ValueList;

        protected ValueIndicators ( string name ) : base( name )
        {
            ValueList = new List< double >( 1 );
        }

        protected ValueIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints )
        {
            ValueList = new List< double >( _categoryConstraints.Count );
        }

        protected override void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints )
        {
            throw new NotImplementedException();
        }
    }
}
