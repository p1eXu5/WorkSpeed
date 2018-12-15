using System;
using System.Collections.Generic;
using NpoiExcel.Attributes;
using WorkSpeed.ActionModels;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public abstract class ActionImportModel : WithEmployeeImportModel
    {
        [Header("Дата")]        public DateTime DateTime { get; set; }

        [Header("Номер документа")]     public string DocumentNumber { get; set; }
        [Header("Документ")]            public string DocumentName { get; set; }

        [Header("Время операции, сек.")]    public int OperationDuration { get; set; }


        public virtual EmployeeAction ToEmployeeAction( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
