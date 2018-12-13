using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class CargoIndicators : QuantityIndicators
    {
        public CargoIndicators ( string name ) : base( name ) { }
        public CargoIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints ) { }
        protected override void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints )
        {
            throw new NotImplementedException();
        }

        protected override void Add ( EmployeeAction employeeAction )
        {
            throw new NotImplementedException();
        }
    }
}
