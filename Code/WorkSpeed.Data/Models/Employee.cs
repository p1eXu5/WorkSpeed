using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models
{
    public class Employee
    {
        [ MaxLength (7), MinLength (7), Required ]
        public string Id { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsSmoker { get; set; }
        public DateTime? ProbationEnd { get; set; }

        [ Required ]
        public Position Position { get; set; }

        [ Required ]
        public Rank Rank { get; set; }

        [ Required ]
        public Appointment Appointment { get; set; }

        [ Required ]
        public Shift Shift { get; set; }

        public ShortBreakSchedule ShortBreakSchedule { get; set; }

        public List< ReceptionAction > ReceptionActions { get; set; }
        public List< DoubleAddressAction > DoubleAddressActions { get; set; }
        public List< InventoryAction > InventoryActions { get; set; }
        public List< ShipmentAction > ShipmentActions { get; set; }
        public List< OtherAction > OtherActions { get; set; }

        public List< PersonnelFileRecord > PersonnelFileRecordCollection { get; set; }

        public override string ToString ()
        {
            return $"{Id} {Name}";
        }
    }
}
