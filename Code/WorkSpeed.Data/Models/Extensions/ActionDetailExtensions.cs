using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models.ActionDetails;

namespace WorkSpeed.Data.Models.Extensions
{
    public static class ActionDetailExtensions
    {
        public static double Volume ( this WithProductActionDetail detail )
        {
            return detail.ProductQuantity * (detail.Product.ItemVolume ?? 0.0);
        }
    }
}
