using System;
using System.Collections.Generic;

namespace CircleDiagramTest.Models
{
    /// <summary>
    /// [ MinVolume, MaxVolume ) l.
    /// </summary>
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
        public string Name { get; set; }
        public string Abbreviation { get; set; }

        /// <summary>
        /// Minimum volume in liters.
        /// </summary>
        public double? MinVolume { get; set; }

        /// <summary>
        /// Maximum volume in liters.
        /// </summary>
        public double? MaxVolume { get; set; }
    }
}
