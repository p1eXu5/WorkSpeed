using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class ShortBreak
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Periodicity { get; set; }
        public bool IsForSmokers { get; set; }

        public TimeSpan DayOffsetTime { get; set; }
    }
}
