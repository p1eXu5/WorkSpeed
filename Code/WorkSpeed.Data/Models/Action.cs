using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Action
    {
        public int Id { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public Operation Operation { get; set; }
        public Document1C Document { get; set; }
    }
}
