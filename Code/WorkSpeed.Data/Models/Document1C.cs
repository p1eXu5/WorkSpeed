using System;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models
{
    public class Document1C
    {
        [ MaxLength( 10 ), MinLength( 10 )]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
