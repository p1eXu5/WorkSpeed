using System;
using WorkSpeed.Import.Attributes;

namespace WorkSpeed.FileModels
{
    public class BaseImportModel : ImportModel
    {
        [Header("Дата")]        public DateTime DateTime { get; set; }

        [Header("Код сотрудника")]  public string EmployeeId { get; set; }
        [Header("Сотрудник")]       public string Employee { get; set; }

        [Header("Номер документа")]     public string DocumentNumber { get; set; }
        [Header("Документ")]            public string DocumentName { get; set; }

        [Header("Время операции, сек.")]    public int OperationDuration { get; set; }
    }
}
