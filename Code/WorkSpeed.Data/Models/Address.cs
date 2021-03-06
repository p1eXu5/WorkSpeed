﻿using System;
using System.ComponentModel.DataAnnotations;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.Models
{
    public class Address : IEntity
    {
        [ MaxLength( 1 ), MinLength( 1 )]
        public string Letter { get; set; }
        public byte Section { get; set; }
        public byte Row { get; set; }
        public byte Shelf { get; set; }
        public byte Box { get; set; }

        public BoxTypes BoxType { get; set; }

        public Single? Length { get; set; }
        public Single? Width { get; set; }
        public Single? Height { get; set; }

        public double? Volume { get; set; }

        public Single? MaxWeight { get; set; }

        public Single? VolumeCoefficient { get; set; }
        public Single? Complexity { get; set; }

        public Position Position { get; set; }
    }
}
