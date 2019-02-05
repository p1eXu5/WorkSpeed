using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.FileModels
{
    public class ShipmentImportModel : ActionImportModel
    {
        [Header("Вес на сотрудника")]           public double WeightPerEmployee { get; set; }

        [Header("Номерные ГМ на сотрудника")]       public double ClientCargoQuantity { get; set; }
        [Header("Безномерные ГМ на сотрудника")]    public double CommonCargoQuantity { get; set; }
    }
}
