using WorkSpeed.Import.Attributes;

namespace WorkSpeed.FileModels
{
    public class InventoryImportModel : BaseProductivityImportModel
    {
        [Header("Учётное количество")]      public int AccountingQuantity { get; set; }
        [Header("Фактическое количество")]  public int ActualQuantity { get; set; }

        [Header("Адрес")]                   public string Address { get; set; }
    }
}
