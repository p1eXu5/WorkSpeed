using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Models.ImportModels
{
    public class ImportedGatheringActionDetails : ImportedCommonActionDetails
    {
        public readonly ushort Quantity;
        public readonly ushort SenderIdAddressId;
        public readonly ushort ReceiverIdAddressId;

        public ImportedGatheringActionDetails (ushort productId, ushort quantity, ushort senderId, ushort receiverId)
            : base (productId)
        {
            Quantity = quantity;
            SenderIdAddressId = senderId;
            ReceiverIdAddressId = receiverId;
        }
    }
}
