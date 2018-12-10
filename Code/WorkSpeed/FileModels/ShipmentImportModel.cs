using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public class ShipmentImportModel : BaseImportModel
    {
        [Header("Операция")]                public string Operation { get; set; }

        [Header("Вес на сотрудника")]           public double WeightPerEmployee { get; set; }

        [Header("Номерные ГМ на сотрудника")]       public double ClientCargoQuantity { get; set; }
        [Header("Безномерные ГМ на сотрудника")]    public double CommonCargoQuantity { get; set; }

        public override EmployeeAction GetAction()
        {
            throw new System.NotImplementedException();
        }
    }
}
