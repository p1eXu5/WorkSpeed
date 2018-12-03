using WorkSpeed.Import.Attributes;

namespace WorkSpeed.Import.Models.FileModels
{
    public class ShipmentImportModel : BaseImportModel
    {
        [Header("Операция")]                public string Operation { get; set; }

        [Header("Вес на сотрудника")]           public double WeightPerEmployee { get; set; }

        [Header("Номерные ГМ на сотрудника")]       public double ClientCargoQuantity { get; set; }
        [Header("Безномерные ГМ на сотрудника")]    public double CommonCargoQuantity { get; set; }
    }
}
