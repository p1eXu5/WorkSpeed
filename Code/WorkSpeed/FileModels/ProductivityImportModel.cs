using NpoiExcel.Attributes;

namespace WorkSpeed.FileModels
{
    public class ProductivityImportModel : WithProductActionImportModel
    {
        [Header("Учётное количество")]          public int AccountingQuantity { get; set; }
        [Header("Количество сканов")]           public int ScanQuantity { get; set; }
        [Header("Сканирование транзитов")]      public bool IsClientScanning { get; set; }

        [Header("Адрес-отправитель")]           public string AddressSender { get; set; }
        [Header("Адрес-получатель")]            public string AddressReceiver { get; set; }

        [Header("Вес на сотрудника")]               public double WeightPerEmployee { get; set; }
        [Header("Номерные ГМ на сотрудника")]       public double ClientCargoQuantityt { get; set; }
        [Header("Безномерные ГМ на сотрудника")]    public double CommonCargoQuantity { get; set; }
    }
}
