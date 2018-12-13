using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models
{
    public enum OperationGroups
    {
        Gathering = 1,
        ClientGathering,
        Packing,
        ClientPacking,
        Placing,
        ShopperGathering,
        Replacing,
        Defragmentation,
        Inventory,
        Scanning,
        ClientScanning,
        Shipment,
        Unknown
    }
}
