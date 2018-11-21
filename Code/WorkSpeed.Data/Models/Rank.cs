using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Rank
    {
        [Key]
        public int Number { get; set; }
        public decimal OneHourCost { get; set; }
    }
}
