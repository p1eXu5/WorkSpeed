using System;
using NpoiExcel;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public class ImportModelConverter : ITypeConverter< ImportModel, EmployeeAction >
    {
        private readonly IImportModelVisiter _visiter;

        public ImportModelConverter ( IImportModelVisiter visiter )
        {
            _visiter = visiter ?? throw new ArgumentNullException();
        }

        public EmployeeAction Convert ( ImportModel obj )
        {
            return obj.ToEmployeeAction( _visiter );
        }
    }
}
