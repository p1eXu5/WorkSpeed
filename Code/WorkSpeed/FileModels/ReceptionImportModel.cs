using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels
{
    public class ReceptionImportModel : WithProductActionImportModel
    {
        [Header("Количество сканов")]       public int ScanQuantity { get; set; }
        [Header("Сканирование транзитов")]  public bool IsClientScanning { get; set; }

        [Header("Адрес")]                   public string Address { get; set; }
    }
}
