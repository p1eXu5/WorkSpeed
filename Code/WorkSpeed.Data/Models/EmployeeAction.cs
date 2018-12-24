using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class EmployeeAction
    {
        public int Id { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public DateTime StartTime { get; set; }
        public Document1C Document { get; set; }

        public Operation Operation { get; set; }
        public TimeSpan Duration { get; set; }

        public bool IsGatheringOperation ()
        {
            switch ( Operation.Group.Name )
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

        public bool IsPackingOperation ()
        {
            switch ( Operation.Group.Name )
            {
                case OperationGroups.Packing:
                case OperationGroups.ClientPacking:

                    return true;
            }

            return false;
        }
    }
}
