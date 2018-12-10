using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.Import.Models.ImportModels
{
    public class ImportedInventoryActionDetails : ImportedCommonActionDetails
    {
        public readonly ushort AccountingQuantity;
        public readonly ushort ActualQuantity;
        public readonly ushort AddressId;

        public ImportedInventoryActionDetails (ushort productId, ushort accauntingQuantity, ushort actualQuantity, ushort addressId)
            : base(productId)
        {
            AccountingQuantity = accauntingQuantity;
            ActualQuantity = actualQuantity;
            AddressId = addressId;
        }
    }
}
