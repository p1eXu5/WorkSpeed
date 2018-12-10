using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Employee
    {
        [MaxLength (7), MinLength (7), Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ProbationEnd { get; set; }

        public Position Position { get; set; }
        public Rank Rank { get; set; }
        public Appointment Appointment { get; set; }

        public ICollection<PersonnelFileEntry> PersonnelFileEntryCollection { get; set; }
        public ICollection<EmployeeAction> Actions { get; set; }
    }
}
