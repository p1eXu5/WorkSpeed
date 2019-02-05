using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Interfaces;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public class LineIndicators : ValueIndicators
    {
        public LineIndicators ( string name ) : base( name ) { }
        public LineIndicators ( string name, ICategoryConstraints constraints ) : base( name, constraints ) { }


        protected override void Add ( EmployeeActionBase employeeAction )
        {
            switch ( employeeAction ) {

                case GatheringAction gatheringAction:

                    var valueIndex = CategoryConstraints.GetCategoryNum( gatheringAction.Product );
                    ++ValueList[ valueIndex ];
                    break;
                    
            }

        }
    }
}
