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
    public class CargoIndicators : QuantityIndicators
    {
        public CargoIndicators ( string name ) : base( name ) { }
        public CargoIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints ) { }
        protected override void OnChangeCategoryConstraints ( ICategoryConstraints categoryConstraints )
        {
            throw new NotImplementedException();
        }

        protected override void Add ( EmployeeActionBase employeeAction )
        {
            throw new NotImplementedException();
        }
    }
}
