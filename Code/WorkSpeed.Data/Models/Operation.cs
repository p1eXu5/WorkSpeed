﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OperationGroups OperationGroup { get; set; }
        public float Complexity { get; set; }
    }
}
