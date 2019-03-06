using System;
using System.Collections.Generic;
using System.Text;

namespace WorkSpeed.Data.Models.Enums
{
    public enum OperationGroups
    {
        Undefined,
        Gathering = 1,
        BuyerGathering,
        Packing,
        Placing,
        Defragmentation,
        Inventory,
        Reception,
        Shipment,
        Other,
        Time = 100,
        SortByDefault = 200,
    }
}
