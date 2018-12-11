using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public class ReceptionImportModel : ActionProductivityImportModel
    {
        [Header("Количество")]              public int ActualQuantity { get; set; }
        [Header("Количество сканов")]       public int ScanQuantity { get; set; }
        [Header("Сканирование транзитов")]  public bool IsClientScanning { get; set; }

        [Header("Адрес")]                   public string Address { get; set; }

        public override EmployeeAction GetAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
