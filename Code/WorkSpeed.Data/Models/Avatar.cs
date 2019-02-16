using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class Avatar
    {
        public int Id { get; set; }
        public byte[] Picture { get; set; }
        public int Stride { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public List< Employee > Employees { get; set; }
    }
}
