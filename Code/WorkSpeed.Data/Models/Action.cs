using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Action
    {
        public int Id { get; set; }

        [Required]
        public Employee Employee { get; set; }

        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }

        public string Document1C { get; set; }
        public short? ItemCount { get; set; }
        public short? LineCount { get; set; }
        public float? Volume { get; set; }
        public short? ScanCount { get; set; }
        public Adress SenderAdress { get; set; }
        public Adress ReceiverAdress { get; set; }

        public Operation Operation { get; set; }
    }
}
