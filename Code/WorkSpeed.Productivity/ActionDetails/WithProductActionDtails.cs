using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity.ActionDetails
{
    public class WithProductActionDtails : TimeActionDetails
    {
        protected readonly ICategoryFilter _filter;

        public WithProductActionDtails ( ICategoryFilter filter )
        {
            _filter = filter ?? throw new ArgumentNullException( nameof( filter ), "ICategoryFilter cannot be null." );

            Weight = new List< double >( _filter.Count );
            Volume = new List< double >( _filter.Count );
            Lines = new List< int >( _filter.Count );
            Quantity = new List< int >( _filter.Count );
        }

        public override void AddDetails ( EmployeeAction action, TimeSpan pause )
        {
            base.AddDetails( action, pause );

            if ( !(action is WithProductAction withProductAction) ) return;

            var product = withProductAction.Product;
            var category = _filter.GetCategory( product );

            Weight[ category ] += product.Weight + withProductAction.ProductQuantity;
            Volume[ category ] += product.Volume + withProductAction.ProductQuantity;
            Quantity[ category ] = withProductAction.ProductQuantity;
            Lines[ category ]++;
        }

        public List< double > Weight { get; private set; }
        public List< double > Volume { get; private set; }
        public List< int > Lines { get; private set; }
        public List< int > Quantity { get; private set; }
    }
}
