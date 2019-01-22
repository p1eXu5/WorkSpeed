using System.Collections.Generic;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.Models.Actions
{
    public class DoubleAddressAction : EmployeeActionBase
    {
        public List< DoubleAddressDetail > DoubleAddressDetails { get; set; }
    }
}
