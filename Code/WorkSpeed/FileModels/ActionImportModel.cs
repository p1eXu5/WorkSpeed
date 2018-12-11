using System;
using System.Collections.Generic;
using WorkSpeed.ActionModels;
using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public abstract class ActionImportModel : ImportModel
    {
        [Header("Дата")]        public DateTime DateTime { get; set; }

        [Header("Код сотрудника")]  public string EmployeeId { get; set; }
        [Header("Сотрудник")]       public string Employee { get; set; }

        [Header("Номер документа")]     public string DocumentNumber { get; set; }
        [Header("Документ")]            public string DocumentName { get; set; }

        [Header("Время операции, сек.")]    public int OperationDuration { get; set; }

        public override Employee GetEmployee()
        {
            return new Employee() {Id = EmployeeId, Name = Employee};
        }
    }
}
