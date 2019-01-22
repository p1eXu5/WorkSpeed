using System;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models
{
    public class Document1C
    {
        [ MaxLength( 10 )]
        public string Id { get; set; }

        [ Required ]
        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
