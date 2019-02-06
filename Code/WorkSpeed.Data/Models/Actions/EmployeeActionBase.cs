using System;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models.Actions
{
    public abstract class EmployeeActionBase : IKeyedEntity< int >
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
