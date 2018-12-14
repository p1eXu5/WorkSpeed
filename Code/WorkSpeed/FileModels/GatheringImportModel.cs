using System.Collections.Generic;
using WorkSpeed.ActionModels;
using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public class GatheringImportModel : ActionProductImportModel
    {
        [Header("Операция")]    public string Operation { get; set; }

        [Header("Количество")]      public int ProductQuantity { get; set; }

        [Header("Адрес-отправитель")]   public string AddressSender { get; set; }
        [Header("Адрес-получатель")]    public string AddressReceiver { get; set; }

        public override EmployeeAction GetAction()
        {
            return new GatheringAction ();
        }
    }
}
