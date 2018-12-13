using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class WeightIndicators : ValueIndicators
    {
        public WeightIndicators ( string name ) : base( name ) { }
        public WeightIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints ) { }

        protected override void Add ( EmployeeAction employeeAction )
        {
            throw new NotImplementedException();
        }
    }
}
