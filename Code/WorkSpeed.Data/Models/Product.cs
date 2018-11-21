using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; }

        public Product Parent { get; set; }
        public Complexity Complexity { get; set; }
    }
}
