using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class WorkDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public WorkDayStates State { get; set; }
    }
}
