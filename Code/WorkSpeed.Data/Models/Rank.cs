using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models
{
    public class Rank
    {
        [Key]
        public int Number { get; set; }
        public decimal OneHourCost { get; set; }
    }
}
