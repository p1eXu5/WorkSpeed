using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity.ActionDetails
{
    public class WithProductActionDetails : TimeActionDetails
    {
        protected readonly ICategoryFilter _filter;

        public WithProductActionDetails ( ICategoryFilter filter )
        {
            _filter = filter ?? throw new ArgumentNullException( nameof( filter ), "ICategoryFilter cannot be null." );

            var categoryCount = _filter.CategoryList.Count();

            Weight = new double[ categoryCount ];
            Volume = new double[ categoryCount ];
            Lines = new int[ categoryCount ];
            Quantity = new int[ categoryCount ];
        }

        public override void AddDetails ( EmployeeActionBase action, TimeSpan pause )
        {
            base.AddDetails( action, pause );

            //if ( !(action is WithProductAction withProductAction) ) return;

            //var product = withProductAction.Product;
            //var category = _filter.GetCategoryIndex( product );

            //if ( category < 0) return;

            //Weight[ category ] += product.Weight * withProductAction.ProductQuantity;
            //Volume[ category ] += product.GetVolume() * withProductAction.ProductQuantity;
            //Quantity[ category ] += withProductAction.ProductQuantity;
            //Lines[ category ]++;
        }

        public double[] Weight { get; private set; }
        public double[] Volume { get; private set; }
        public int[] Lines { get; private set; }
        public int[] Quantity { get; private set; }

        public Dictionary< Category, int > GetQuantityMap ()
        {
            var res = new Dictionary< Category, int >();
            var categories = _filter.CategoryList.ToArray();

            for ( int i = 0; i < categories.Length; i++ ) {
                res.Add( categories[ i ], Quantity[ i ] );
            }

            return res;
        }

        public Dictionary< Category, int > GetLinesMap ()
        {
            var res = new Dictionary<Category, int>();
            var categories = _filter.CategoryList.ToArray();

            for ( int i = 0; i < categories.Length; i++ )
            {
                res.Add( categories[ i ], Lines[ i ] );
            }

            return res;
        }

        public Dictionary< Category, double > GetWeightMap ()
        {
            var res = new Dictionary<Category, double>();
            var categories = _filter.CategoryList.ToArray();

            for ( int i = 0; i < categories.Length; i++ )
            {
                res.Add( categories[ i ], Weight[ i ] );
            }

            return res;
        }


        public Dictionary< Category, double > GetVolumeMap ()
        {
            var res = new Dictionary<Category, double>();
            var categories = _filter.CategoryList.ToArray();

            for ( int i = 0; i < categories.Length; i++ )
            {
                res.Add( categories[ i ], Volume[ i ] );
            }

            return res;
        }
    }
}
