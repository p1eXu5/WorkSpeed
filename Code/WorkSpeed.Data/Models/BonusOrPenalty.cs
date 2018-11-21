using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class BonusOrPenalty
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [Required, MaxLength(96)]
        public string Document1C { get; set; }

        [MaxLength(96)]
        public string MarkdownDocument { get; set; }

        public BonusesAndPenaltiesKinds KindOf { get; set; }
        public decimal Sum { get; set; }
        public string Comment { get; set; }
    }
}
