using System;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models
{
    public class PersonnelFileRecord
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Comment { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public Appointment Appointment { get; set; }
        public Rank Ranks { get; set; }
        public Position Positions { get; set; }
    }
}
