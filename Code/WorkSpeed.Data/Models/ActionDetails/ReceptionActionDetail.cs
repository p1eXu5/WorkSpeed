
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Models.ActionDetails
{
    public class ReceptionActionDetail : SingleAddressDetail
    {
        public short ScanQuantity { get; set; }
        public bool IsClientScanning { get; set; }

        public string ReceptionActionId { get; set; }
        public ReceptionAction ReceptionAction { get; set; }
    }
}
