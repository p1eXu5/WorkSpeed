using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Business.FileModels.Converters;

namespace WorkSpeed.Business.FileModels
{
    public class GatheringImportModel : WithProductActionImportModel
    {
        [Header("Адрес-отправитель")]
        [Header( "АдресОтправитель" )]
        public string AddressSender { get; set; }

        [Header("Адрес-получатель")]
        [Header( "АдресПолучатель" )]
        public string AddressReceiver { get; set; }
    }
}
