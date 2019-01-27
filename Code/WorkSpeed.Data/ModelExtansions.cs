using System;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data
{
    public static class ModelExtansions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeAction"></param>
        /// <returns></returns>
        public static bool IsGatheringOperation ( this EmployeeActionBase employeeAction )
        {
            var operationGroup = employeeAction?.Operation?.Group?.Name ?? throw new ArgumentNullException();

            switch ( operationGroup )
            {

                case OperationGroups.Gathering:
                case OperationGroups.ClientGathering:
                case OperationGroups.ShopperGathering:
                case OperationGroups.Packing:
                case OperationGroups.ClientPacking:
                case OperationGroups.Defragmentation:
                case OperationGroups.Placing:
                case OperationGroups.Replacing:

                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeAction"></param>
        /// <returns></returns>
        public static bool IsShipmentOperation ( this EmployeeActionBase employeeAction )
        {
            var operationGroup = employeeAction?.Operation?.Group?.Name ?? throw new ArgumentNullException();

            switch ( operationGroup )
            {
                case OperationGroups.Shipment:

                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeAction"></param>
        /// <returns></returns>
        public static OperationGroups GetOperationGroup ( this EmployeeActionBase employeeAction )
        {
            return employeeAction?.Operation?.Group?.Name ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Volume in liters</returns>
        public static double GetVolume ( this Product product )
        {
            if ( product == null ) throw new ArgumentNullException();

            return product.ItemHeight * product.ItemWidth * product.ItemLength / 1000;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeAction"></param>
        /// <returns></returns>
        public static DateTime EndTime ( this EmployeeActionBase employeeAction )
        {
            if ( employeeAction == null ) throw new ArgumentNullException( nameof( employeeAction ), "EmployeeAction cannot be null." );

            return employeeAction.StartTime.Add( employeeAction.Duration );
        }
    }
}
