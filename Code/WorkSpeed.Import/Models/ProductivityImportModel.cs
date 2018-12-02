using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Import.Attributes;

namespace WorkSpeed.Import.Models
{
    public class ProductivityImportModel : BaseProductivityImportModel
    {
        [Header("Операция")]                public string Operation { get; set; }

        [Header("Учётное количество")]          public int AccountingQuantity { get; set; }
        [Header("Фактическое количество")]      public int ActualQuantity { get; set; }
        [Header("Количество сканов")]           public int ScanQuantity { get; set; }
        [Header("Сканирование транзитов")]      public bool IsClientScanning { get; set; }

        [Header("Адрес-отправитель")]           public string AddressSender { get; set; }
        [Header("Адрес-получатель")]            public string AddressReceiver { get; set; }

        [Header("Вес на сотрудника")]               public double WeightPerEmployee { get; set; }
        [Header("Номерные ГМ на сотрудника")]       public double ClientCargoQuantityt { get; set; }
        [Header("Безномерные ГМ на сотрудника")]    public double CommonCargoQuantity { get; set; }
    }
}
