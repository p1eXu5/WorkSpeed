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

        public float ItemLength { get; set; }
        public float ItemWidth  { get; set; }
        public float ItemHeight { get; set; }

        public float CartonLength { get; set; }
        public float CartonWidth  { get; set; }
        public float CartonHeight { get; set; }

        public int CartonQuantity { get; set; }

        public float Weight { get; set; }
        public float Volume { get; set; }

        public float GatheringComplexity { get; set; }
        public float PackagingComplexity { get; set; }
        public float ScanningComplexity  { get; set; }
        public float InventoryComplexity { get; set; }
        public float PlacingComplexity   { get; set; }

        public Product Parent { get; set; }

    }
}
