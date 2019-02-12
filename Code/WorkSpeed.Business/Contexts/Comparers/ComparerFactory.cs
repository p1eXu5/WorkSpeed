using System.Collections.Generic;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Comparers
{
    public static class ComparerFactory
    {
        private static IEqualityComparer< Employee > _employeeComparer;
        private static IEqualityComparer< Product > _productComparer;
        private static IEqualityComparer< Address > _addressComparer;
        private static IEqualityComparer< EmployeeActionBase > _employeeActionBaseComparer;
        private static IEqualityComparer< ShipmentAction > _shipmentActionComperer;


        public static IEqualityComparer< Employee > EmployeeComparer 
            => _employeeComparer ?? (_employeeComparer = GetEmployeeComparer());

        public static IEqualityComparer< Product > ProductComparer 
            => _productComparer ?? (_productComparer = GetProductComparer());

         public static IEqualityComparer< Address > AddressComparer 
            => _addressComparer ?? (_addressComparer = GetAddressComparer());

        public static IEqualityComparer< EmployeeActionBase > EmployeeActionBaseComparer 
            => _employeeActionBaseComparer ?? (_employeeActionBaseComparer = GetEmployeeActionBaseComparer() );

        public static IEqualityComparer< ShipmentAction > ShipmentActionComparer 
            => _shipmentActionComperer ?? ( _shipmentActionComperer = GetShipmentActionComperer() );

        #region Private Methods

        private static IEqualityComparer< Employee > GetEmployeeComparer()
            => new EntityEqualityComparer< Employee, string >();

        private static IEqualityComparer< Product > GetProductComparer()
            => new EntityEqualityComparer< Product, int >();

        private static IEqualityComparer< Address > GetAddressComparer()
            => new AddressEqualityComparer();

        private static IEqualityComparer< EmployeeActionBase > GetEmployeeActionBaseComparer()
            => new EntityEqualityComparer< EmployeeActionBase, string >();

        private static IEqualityComparer< ShipmentAction > GetShipmentActionComperer()
            => new ShipmentActionComparer();

        #endregion

    }
}
