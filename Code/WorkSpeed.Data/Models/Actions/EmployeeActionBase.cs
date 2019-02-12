using System;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models.Actions
{
    public abstract class EmployeeActionBase : IKeyedEntity< string >
    {
        [ MinLength(11) ]
        public string Id { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string DocumentName { get; set; }

        [Required]
        public Employee Employee { get; set; }

        [Required]
        public Operation Operation { get; set; }
    }
}
