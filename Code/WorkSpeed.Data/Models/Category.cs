using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public double? MinVolume { get; set; }

        /// <summary>
        /// if it's null, than MaxVolume equal to positive infinity or
        /// next MinVolume.
        /// </summary>
        public double MaxVolume { get; set; }
    }
}
