using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public abstract class EmployeeAction
    {
        public int Id { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public DateTime StartTime { get; set; }
        public Document1C Document1C { get; set; }

        public Operation Operation { get; set; }
        public TimeSpan Duration { get; set; }

        
    }
}
