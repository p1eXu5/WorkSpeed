using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(196)]
        public string Name { get; set; }

        public bool IsGroup { get; set; }

        public ProductDims ProductDims { get; set; }

        public Product Parent { get; set; }
        public Complexity Complexity { get; set; }

        public float Volume => ProductDims.Volume;
        public float Weight => ProductDims.ItemWeight;
    }
}
