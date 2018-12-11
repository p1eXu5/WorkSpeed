using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    class TimeIndicators : ProductivityIndicators
    {
        public TimeSpan GatheringTime { get; set; }
        public TimeSpan ClientGatheringTime { get; set; }
        public TimeSpan ScanningTime { get; set; }
        public TimeSpan ClientScanningTime { get; set; }
        public TimeSpan DefragmentationTime { get; set; }
        public TimeSpan PlacingTime { get; set; }
        public TimeSpan InventorizationTime { get; set; }
        public TimeSpan ShipmentTime { get; set; }
        public TimeSpan NonProductiveTime { get; set; }

        public override string GetName()
        {
            throw new NotImplementedException();
        }
    }
}
