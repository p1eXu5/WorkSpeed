using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity.ActionDetails
{
    public class WithScansActionDetails : WithProductActionDetails
    {
        public WithScansActionDetails ( ICategoryFilter filter ) : base( filter )
        {
            Scans = new int[ _filter.CategoryList.Count() ];
        }

        public override void AddDetails ( EmployeeActionBase action, TimeSpan pause )
        {
            base.AddDetails( action, pause );

            if ( !( action is ReceptionAction reception ) ) return;

            //Scans[ _filter.GetCategoryIndex( reception.Product ) ] += reception.ScanQuantity;
        }

        public int[] Scans { get; private set; }

        public Dictionary<Category, int> GetScansMap ()
        {
            var res = new Dictionary<Category, int>();
            var categories = _filter.CategoryList.ToArray();

            for ( int i = 0; i < categories.Length; i++ )
            {
                res.Add( categories[ i ], Scans[ i ] );
            }

            return res;
        }
    }
}
