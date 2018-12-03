using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Manager.Models
{
    public enum TimePeriodTypes
    {
        GatheringTime,
        ClientGatheringTime,
        ScanningTime,
        ClientScanningTime,
        DefragmentationTime,
        PlacingTime,
        InventorizationTime,
        ShipmentTime,
        NonProductiveTime
    }
}
