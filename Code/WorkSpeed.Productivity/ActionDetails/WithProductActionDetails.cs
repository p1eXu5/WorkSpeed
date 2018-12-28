using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity.ActionDetails
{
    public class WithProductActionDetails : TimeActionDetails
    {
        protected readonly ICategoryFilter _filter;

        public WithProductActionDetails ( ICategoryFilter filter )
        {
            throw new NotImplementedException();
            _filter = filter ?? throw new ArgumentNullException( nameof( filter ), "ICategoryFilter cannot be null." );

            //Weight = new List< double >( _filter.Count );
            //Volume = new List< double >( _filter.Count );
            //Lines = new List< int >( _filter.Count );
            //Quantity = new List< int >( _filter.Count );
        }

        public override void AddDetails ( EmployeeAction action, TimeSpan pause )
        {
            throw new NotImplementedException();
            base.AddDetails( action, pause );

            if ( !(action is WithProductAction withProductAction) ) return;

            var product = withProductAction.Product;
            //var category = _filter.GetCategoryIndex( product );

           // Weight[ category ] += product.Weight + withProductAction.ProductQuantity;
            //Volume[ category ] += product.Volume + withProductAction.ProductQuantity;
           // Quantity[ category ] = withProductAction.ProductQuantity;
            //Lines[ category ]++;
        }

        public List< double > Weight { get; private set; }
        public List< double > Volume { get; private set; }
        public List< int > Lines { get; private set; }
        public List< int > Quantity { get; private set; }
    }
}
