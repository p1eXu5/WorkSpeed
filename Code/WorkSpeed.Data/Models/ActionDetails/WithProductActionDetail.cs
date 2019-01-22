
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class WithProductActionDetail : ActionDetailBase
    {
        [Required]
        public Product Product { get; set; }
        public int ProductQuantity { get; set; }
    }
}
