using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Data.Models
{
    public class Category
    {
        public Category ()
        { }

        public Category ( int minVolume, int maxVolume )
        {
            MinVolume = minVolume;
            MaxVolume = maxVolume;
        }

        public Category ( double minVolume, double maxVolume )
        {
            MinVolume = minVolume;
            MaxVolume = maxVolume;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Minimum volume in liters.
        /// </summary>
        public double MinVolume { get; set; }

        /// <summary>
        /// Maximum volume in liters.
        /// </summary>
        public double MaxVolume { get; set; }
    }
}
