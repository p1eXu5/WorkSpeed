using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Models;
using WorkSpeed.Business.Models.Productivity;
using WorkSpeed.Data;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.Productivity.Builders
{
    public class ProductivityDirector : IProductivityDirector
    {
        private readonly Dictionary< OperationGroups, IProductivityBuilder > _builders;

        public ProductivityDirector ()
        {
            _builders= new Dictionary< OperationGroups, IProductivityBuilder > {
                [ OperationGroups.Gathering ] = new WithProductProductivityBuilder(),
                [ OperationGroups.Reception ] = _builders[ OperationGroups.Gathering ],
                [ OperationGroups.Inventory ] = _builders[ OperationGroups.Gathering ],
                [ OperationGroups.Shipment ] = new ShipmentProductivityBuilder(),
                [ OperationGroups.Other ] = new OtherProductivityBuilder(),
            };
        }

        public IEnumerable< IProductivity > GetProductivities ( IEnumerable< EmployeeActionBase > actions, OperationThresholds thresholds )
        {
            var withProductActions = new List< EmployeeActionBase >();

            foreach ( var action in actions ) {
                _builders[ action.GetOperationGroup() ].Add( action, thresholds );
            }

            yield return _builders[ OperationGroups.Gathering ].GetProductivity;
            yield return _builders[ OperationGroups.Shipment ].ProductivityCollection;
            yield return _builders[ OperationGroups.Other ].ProductivityCollection;
        }
    }
}
