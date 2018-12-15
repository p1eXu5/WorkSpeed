using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class OperationGroup
    {
        public int Id { get; set; }
        public OperationGroups Name { get; set; }
        public float Complexity { get; set; }
    }
}
