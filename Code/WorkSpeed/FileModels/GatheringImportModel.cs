using WorkSpeed.Import.Attributes;

namespace WorkSpeed.FileModels
{
    public class GatheringImportModel : BaseProductivityImportModel
    {
        [Header("Операция")]    public string Operation { get; set; }

        [Header("Количество")]      public int ProductQuantity { get; set; }

        [Header("Адрес-отправитель")]   public string AddressSender { get; set; }
        [Header("Адрес-получатель")]    public string AddressReceiver { get; set; }
    }
}
