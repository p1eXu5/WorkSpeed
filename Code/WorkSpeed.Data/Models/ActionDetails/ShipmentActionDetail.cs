
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class ShipmentActionDetail : ActionDetailBase
    {
        public float? Weight { get; set; }
        public float? Volume { get; set; }
        public float? ClientCargoQuantity { get; set; }
        public float? CommonCargoQuantity { get; set; }

        public int ShipmentActionId { get; set; }
        public ShipmentAction ShipmentAction { get; set; }
    }
}
