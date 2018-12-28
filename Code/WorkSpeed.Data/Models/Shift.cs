using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan ShiftDuration { get; set; }
        public TimeSpan LunchDuration { get; set; }
        public TimeSpan RestTime { get; set; }
    }
}
