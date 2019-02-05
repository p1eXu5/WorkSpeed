using Agbm.NpoiExcel.Attributes;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class GatheringImportModel : WithProductActionImportModel
    {
        [Header("Адрес-отправитель")]
        [Header( "АдресОтправитель" )]
        public string AddressSender { get; set; }

        [Header("Адрес-получатель")]
        [Header( "АдресПолучатель" )]
        public string AddressReceiver { get; set; }

        public override object Convert ( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
