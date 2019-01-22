using System.Collections.Generic;

namespace WorkSpeed.Data.Models
{
    public class OperationGroup
    {
        public int Id { get; set; }
        public OperationGroups Name { get; set; }
        public float Complexity { get; set; }

        public List< Operation > Operations { get; set; }
    }
}
