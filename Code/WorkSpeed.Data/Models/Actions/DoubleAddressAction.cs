﻿using System.Collections.Generic;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.Models.Actions
{
    public class DoubleAddressAction : EmployeeActionBase
    {
        public List< DoubleAddressActionDetail > DoubleAddressDetails { get; set; }
    }
}
