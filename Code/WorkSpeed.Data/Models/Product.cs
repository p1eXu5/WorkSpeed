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
        public float Weight { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Volume { get; set; }

        public bool IsGroup { get; set; }

        public Product Parent { get; set; }
        public Complexity Complexity { get; set; }
    }
}
