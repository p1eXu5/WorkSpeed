using System;

namespace WorkSpeed.Data.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan Lunch { get; set; }
        public TimeSpan? RestTime { get; set; }
    }
}
