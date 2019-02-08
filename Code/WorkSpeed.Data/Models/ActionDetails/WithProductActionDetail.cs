
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class WithProductActionDetail : ActionDetailBase
    {
        public int ProductQuantity { get; set; }

        [Required]
        public Product Product { get; set; }
    }
}
