using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class ProductDims
    {
        public int ProductId { get; set; }

        public float ItemLength { get; set; }
        public float ItemWidth { get; set; }
        public float ItemHeight { get; set; }

        public float CartonLength { get; set; }
        public float CartonWidth { get; set; }
        public float CartonHeight { get; set; }

        public int CartonQuantity { get; set; }

        public float Weight { get; set; }
        public float Volume { get; set; }
    }
}
