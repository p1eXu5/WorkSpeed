using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Address
    {
        public char[] Letter { get; set; } = new char[1];
        public byte Section { get; set; }
        public byte Row { get; set; }
        public byte Shelf { get; set; }
        public byte Box { get; set; }

        public BoxTypes BoxType { get; set; }

        public Single? Lenght { get; set; }
        public Single? Width { get; set; }
        public Single? Height { get; set; }

        public double? Volume { get; set; }

        public Single? MaxWeight { get; set; }

        public Single? VolumeCoefficient { get; set; }
        public Single? Complexity { get; set; }

        public Position Position { get; set; }
    }
}
