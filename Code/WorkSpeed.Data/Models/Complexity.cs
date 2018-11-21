using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Complexity
    {
        public int Id { get; set; }
        public float GatheringComplexity { get; set; }
        public float PackagingComplexity { get; set; }
        public float ScanningComplexity { get; set; }
        public float InventoryComplexity { get; set; }
        public float PlacingComplexity { get; set; }
    }
}
