using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Address
    {
        public char Letter { get; set; }
        public byte Section { get; set; }
        public byte Row { get; set; }
        public byte Shelf { get; set; }
        public byte Box { get; set; }

        public BoxTypes BoxType { get; set; }

        public float Lenght { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public double Volume { get; set; }

        public float MaxWeight { get; set; }

        public float VolumeCoefficient { get; set; }
        public float Complexity { get; set; }

        public Position Position { get; set; }
    }
}
