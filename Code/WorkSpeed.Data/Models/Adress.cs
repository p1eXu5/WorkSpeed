using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public class Adress
    {
        public char Letter { get; set; }
        public byte Section { get; set; }
        public byte Row { get; set; }
        public byte Shelf { get; set; }
        public byte Cell { get; set; }

        public CellTypes CellType { get; set; }

        public float Lenght { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public float MaxWeight { get; set; }

        public float VolumeCoefficient { get; set; }
        public float Complexity { get; set; }

        public Position Position { get; set; }
    }
}
