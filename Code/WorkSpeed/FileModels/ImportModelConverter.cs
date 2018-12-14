using System;
using NpoiExcel;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class ImportModelConverter : ITypeConverter< ImportModel, EmployeeAction >
    {
        private readonly IImportModelVisitor _visitor;

        public ImportModelConverter ( IImportModelVisitor visitor )
        {
            _visitor = visitor ?? throw new ArgumentNullException();
        }

        public EmployeeAction Convert ( ImportModel obj )
        {
            return obj.ToEmployeeAction( _visitor );
        }
    }
}
