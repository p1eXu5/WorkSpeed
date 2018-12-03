using WorkSpeed.Import.Attributes;

namespace WorkSpeed.Import.Models.FileModels
{
    public class ReceptionImportModel : BaseProductivityImportModel
    {
        [Header("Количество")]              public int ActualQuantity { get; set; }
        [Header("Количество сканов")]       public int ScanQuantity { get; set; }
        [Header("Сканирование транзитов")]  public bool IsClientScanning { get; set; }

        [Header("Адрес")]                   public string Address { get; set; }
    }
}
