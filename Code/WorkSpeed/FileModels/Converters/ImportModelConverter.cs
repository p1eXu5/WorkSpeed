using System;
using System.Collections.Generic;
using Agbm.NpoiExcel;
using WorkSpeed.Business.FileModels;
using WorkSpeed.Business.FileModels.Contracts;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels.Converters
{
    public class ImportModelConverter : ITypeConverter< IImportModel, IEntity >
    {
        private readonly IImportModelVisitor _visitor;

        public ImportModelConverter ()
        {
            _visitor = new ImportModelVisitor();
        }

        public ImportModelConverter ( IImportModelVisitor visitor )
        {
            _visitor = visitor ?? throw new ArgumentNullException();
        }

        public IEntity Convert ( IImportModel obj )
        {
            return obj.Accept( _visitor );
        }
    }
}
