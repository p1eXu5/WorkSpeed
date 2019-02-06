using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Constraints;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.ProductivityIndicatorsModels
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
