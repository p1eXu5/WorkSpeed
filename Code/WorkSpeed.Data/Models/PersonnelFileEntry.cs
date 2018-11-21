using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace WorkSpeed.Data.Models
{
    public class PersonnelFileEntry
    {
        public int Id { get; set; }
        public DateTime EntryDate { get; set; }
        public string Comment { get; set; }

        public Employee Employee { get; set; }
        public Appointment Appointment { get; set; }
        public Rank Ranks { get; set; }
        public Position Positions { get; set; }
    }
}
