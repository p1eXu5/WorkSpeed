using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data
{
    public static class ModelExtansions
    {
        public static bool IsGatheringOperation ( this EmployeeAction employeeAction )
        {
            var operationGroup = employeeAction?.Operation?.Group?.Name ?? throw new ArgumentNullException();

            switch ( operationGroup )
            {

                case OperationGroups.Gathering:
                case OperationGroups.ClientGathering:
                case OperationGroups.ShopperGathering:
                case OperationGroups.Packing:
                case OperationGroups.ClientPacking:

                    return true;
            }

            return false;
        }

        public static bool IsPackingOperation ( this EmployeeAction employeeAction )
        {
            var operationGroup = employeeAction?.Operation?.Group?.Name ?? throw new ArgumentNullException();

            switch ( operationGroup )
            {
                case OperationGroups.Packing:
                case OperationGroups.ClientPacking:

                    return true;
            }

            return false;
        }

        public static OperationGroups OperationGroup ( this EmployeeAction employeeAction )
        {
            return employeeAction?.Operation?.Group?.Name ?? throw new ArgumentNullException();
        }
    }
}
