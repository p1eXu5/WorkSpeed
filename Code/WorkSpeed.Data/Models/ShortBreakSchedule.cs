using System;

namespace WorkSpeed.Data.Models
{
    public class ShortBreakSchedule : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TimeSpan Duration { get; set; }
        public TimeSpan Periodicity { get; set; }
        public TimeSpan FirstBreakTime { get; set; }
    }
}
