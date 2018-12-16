using System;
using System.Collections.Generic;
using System.Text;
using WorkSpeed.Data.Interfaces;

namespace WorkSpeed.Data.Models
{
    public class Operation : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Complexity { get; set; }

        public int OperationGroupId { get; set; }
        public OperationGroup Group { get; set; }
    }
}
