using System;
using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.Business.FileModels
{
    public abstract class ActionImportModel : WithEmployeeImportModel
    {
        [Header("Дата")]                    public DateTime StartTime { get; set; }

        [Header("Номер документа")]         public string DocumentNumber { get; set; }
        [Header("Документ")]                public string DocumentName { get; set; }

        [Header("Операция")]                public string Operation { get; set; }

        [Header("Время операции, сек.")]
        [Header( "ВремяОперации_Сек" )]
                                            public int OperationDuration { get; set; }
    }
}
