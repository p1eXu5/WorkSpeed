using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkSpeed.Data.Models
{
    public class Product : IEntity, IKeyedEntity< int >
    {
        public int Id { get; set; }

        [MaxLength(196)]
        public string Name { get; set; }

        public bool IsGroup { get; set; }

        /// <summary>
        /// Item lenght in cm.
        /// </summary>
        public float? ItemLength { get; set; }

        /// <summary>
        /// Item width in cm.
        /// </summary>
        public float? ItemWidth  { get; set; }

        /// <summary>
        /// Item height in cm.
        /// </summary>
        public float? ItemHeight { get; set; }
    
        /// <summary>
        /// Item weight in cm.
        /// </summary>
        public float? ItemWeight { get; set; }

        /// <summary>
        /// Computed column that is item volume in litters.
        /// </summary>
        public float? ItemVolume { get; set; }

        /// <summary>
        /// Item carton lenght in cm.
        /// </summary>
        public float? CartonLength { get; set; }

        /// <summary>
        /// Item carton width in cm.
        /// </summary>
        public float? CartonWidth  { get; set; }

        /// <summary>
        /// Item carton height in cm.
        /// </summary>
        public float? CartonHeight { get; set; }

        /// <summary>
        /// Item quantity that contained in carton.
        /// </summary>
        public int? CartonQuantity { get; set; }

        /// <summary>
        /// Computed column that is item carton weight in kg.
        /// </summary>
        public float? CartonWeight { get; set; }

        /// <summary>
        /// Computed column that is item carton volume in litters.
        /// </summary>
        public float? CartonVolume { get; set; }

        public float? GatheringComplexity { get; set; }
        public float? PackagingComplexity { get; set; }
        public float? ScanningComplexity  { get; set; }
        public float? InventoryComplexity { get; set; }
        public float? PlacingComplexity   { get; set; }

        public Product Parent { get; set; }

        public List< Product > Children { get; set; }
    }
}
