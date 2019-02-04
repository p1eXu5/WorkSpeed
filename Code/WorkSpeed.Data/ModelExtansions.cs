using System;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

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
            var operationGroup = employeeAction?.Operation?.OperationGroup ?? throw new ArgumentNullException();

            switch ( operationGroup )
            {
                case OperationGroups.Gathering:
                case OperationGroups.Packing:
                case OperationGroups.Defragmentation:
                case OperationGroups.Placing:

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
            var operationGroup = employeeAction?.Operation?.OperationGroup ?? throw new ArgumentNullException();

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
            return employeeAction?.Operation?.OperationGroup ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Volume in liters</returns>
        public static double GetVolume ( this Product product )
        {
            if ( product == null ) throw new ArgumentNullException();

            return (double)(product.ItemHeight ?? 0 * product.ItemWidth ?? 0 * product.ItemLength ?? 0 / 1000.0);
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
