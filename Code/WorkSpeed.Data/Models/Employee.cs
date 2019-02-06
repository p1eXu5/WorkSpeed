using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models
{
    public class Employee : IEntity
    {
        [ MaxLength (7), MinLength (7)]
        public string Id { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsSmoker { get; set; }
        public DateTime? ProbationEnd { get; set; }

        public int? PositionId { get; set; }
        public Position Position { get; set; }
        public Rank Rank { get; set; }
        public Appointment Appointment { get; set; }
        public Shift Shift { get; set; }
        public ShortBreakSchedule ShortBreakSchedule { get; set; }

        public List< ReceptionAction > ReceptionActions { get; set; }
        public List< DoubleAddressAction > DoubleAddressActions { get; set; }
        public List< InventoryAction > InventoryActions { get; set; }
        public List< ShipmentAction > ShipmentActions { get; set; }
        public List< OtherAction > OtherActions { get; set; }

        public override string ToString ()
        {
            return $"{Id} {Name}";
        }
    }
}
