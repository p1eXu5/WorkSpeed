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
    public class LineIndicators : ValueIndicators
    {
        public LineIndicators ( string name ) : base( name ) { }
        public LineIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints ) { }


        protected override void Add ( EmployeeActionBase employeeAction )
        {

        }
    }
}
