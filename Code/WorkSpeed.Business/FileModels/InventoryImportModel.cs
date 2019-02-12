using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.Business.FileModels
{
    public class InventoryImportModel : WithProductActionImportModel
    {
        /// <summary>
        /// Учётное количество.
        /// </summary>
        [Header("Учётное количество")]      public int AccountingQuantity { get; set; }
        [Header("Адрес")]                   public string Address { get; set; }
    }
}
