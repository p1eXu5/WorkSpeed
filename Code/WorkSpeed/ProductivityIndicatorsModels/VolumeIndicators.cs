using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class VolumeIndicators : ValueIndicators
    {
        public VolumeIndicators ( string name ) : base( name ) { }
        public VolumeIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints ) { }

        protected override void Add ( EmployeeAction employeeAction )
        {
            throw new NotImplementedException();
        }
    }
}
