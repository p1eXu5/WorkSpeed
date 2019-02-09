

using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class DoubleAddressDetail : WithProductActionDetail
    {
        public Address SenderAddress { get; set; }
        public Address ReceiverAddress { get; set; }

        public string DoubleAddressActionId { get; set; }
        public DoubleAddressAction DoubleAddressAction { get; set; }
    }
}
