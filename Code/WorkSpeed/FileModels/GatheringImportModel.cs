using System.Collections.Generic;
using NpoiExcel.Attributes;
using WorkSpeed.ActionModels;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public class GatheringImportModel : WithProductActionImportModel
    {
        [Header("Адрес-отправитель")]   public string AddressSender { get; set; }
        [Header("Адрес-получатель")]    public string AddressReceiver { get; set; }
    }
}
