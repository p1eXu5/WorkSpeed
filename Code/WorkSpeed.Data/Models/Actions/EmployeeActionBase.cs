using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models.Actions
{
    public abstract class EmployeeActionBase
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [Required]
        public Document1C Document1C { get; set; }

        [Required]
        public Operation Operation { get; set; }
    }
}
