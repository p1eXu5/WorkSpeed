using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Models.ImportModels
{
    public class ImportedReceptionActionDetails : ImportedCommonActionDetails
    {
        public readonly ushort Quantity;
        public readonly ushort ScanQuantity;
        public readonly ushort AddressId;
        public readonly bool IsClient;

        public ImportedReceptionActionDetails (ushort productId, ushort quantity, ushort scanQuantity, ushort addressId, bool isClient)
            : base(productId)
        {
            Quantity = quantity;
            ScanQuantity = scanQuantity;
            AddressId = addressId;
            IsClient = isClient;
        }
    }
}
