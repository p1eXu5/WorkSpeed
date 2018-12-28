using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity.ActionDetails
{
    public class WithScansActionDetails : WithProductActionDetails
    {
        public WithScansActionDetails ( ICategoryFilter filter ) : base( filter )
        {
            throw new NotImplementedException();
            //Scans = new List< int >( _filter.Count );
        }

        public override void AddDetails ( EmployeeAction action, TimeSpan pause )
        {
            throw new NotImplementedException();

            base.AddDetails( action, pause );

            if ( !( action is ReceptionAction reception ) ) return;

            //Scans[ _filter.GetCategoryIndex( reception.Product ) ] += reception.ScanQuantity;

        }

        public List< int > Scans { get; private set; }
    }
}
