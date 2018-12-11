using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public class InventoryImportModel : ActionProductivityImportModel
    {
        [Header("Учётное количество")]      public int AccountingQuantity { get; set; }
        [Header("Фактическое количество")]  public int ActualQuantity { get; set; }

        [Header("Адрес")]                   public string Address { get; set; }

        public override EmployeeAction GetAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
