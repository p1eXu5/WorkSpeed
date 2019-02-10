
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class InventoryActionDetail : SingleAddressActionDetail
    {
        public int AccountingQuantity { get; set; }

        public string InventoryActionId { get; set; }
        public InventoryAction InventoryAction { get; set; }
    }
}
