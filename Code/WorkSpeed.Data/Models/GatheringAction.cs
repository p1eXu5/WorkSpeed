﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class GatheringAction : WithProductAction
    {
        public Address FastGatheringCellAdress { get; set; }
        public Address DynamicCellAdress { get; set; }
    }
}
