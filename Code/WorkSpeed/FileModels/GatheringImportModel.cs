using NpoiExcel.Attributes;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class GatheringImportModel : WithProductActionImportModel
    {
        [Header("Адрес-отправитель")]   public string AddressSender { get; set; }
        [Header("Адрес-получатель")]    public string AddressReceiver { get; set; }

        public override object Convert ( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
