

using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class DoubleAddressDetail : ActionDetailBase
    {
        public Address SenderCellAdress { get; set; }
        public Address ReceiverCellAdress { get; set; }

        public string DoubleAddressActionId { get; set; }
        public DoubleAddressAction DoubleAddressAction { get; set; }
    }
}
