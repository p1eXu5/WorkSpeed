using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Contexts.ProductivityCalculator.Builders;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.ProductivityCalculator
{
    public class ProductivityCalculator
    {
        private readonly Dictionary< OperationGroups, IProductivityBuilder > _builders;

        public ProductivityCalculator ()
        {
            _builders = new Dictionary< OperationGroups, IProductivityBuilder > {
                [ OperationGroups.Gathering ] = new GatheringProductivityBuilder(),
                [ OperationGroups.Reception ] = new ReceptionProductivityBuilder(),
                [ OperationGroups.Inventory ] = new InventoryProductivityBuilder(),
                [ OperationGroups.Shipment ] = new ShipmentProductivityBuilder(),
                [ OperationGroups.Other ] = new OtherProductivityBuilder()
            };
        }

        public Productivity GetProductivity ( IEnumerable< EmployeeActionBase > employeeActions, OperationThresholds thresholds, ShortBreakSchedule breaks = null, Shift shift = null )
        {
            foreach ( var action in employeeActions ) {
                
            }

            throw new NotImplementedException();
        }
    }
}
