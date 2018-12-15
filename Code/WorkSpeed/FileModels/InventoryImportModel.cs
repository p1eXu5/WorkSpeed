using NpoiExcel.Attributes;

namespace WorkSpeed.FileModels
{
    public class InventoryImportModel : WithProductActionImportModel
    {
        [Header("Учётное количество")]      public int AccountingQuantity { get; set; }
        [Header("Адрес")]                   public string Address { get; set; }
    }
}
