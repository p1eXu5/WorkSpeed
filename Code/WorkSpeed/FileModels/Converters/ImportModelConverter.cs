using System;
using NpoiExcel;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels.Converters
{
    public class ImportModelConverter< TImport, TDataBase > : ITypeConverter< TImport, TDataBase >
        where   TImport : ImportModel
        where TDataBase : new()
    {
        private readonly IImportModelVisitor _visitor;

        public ImportModelConverter ( IImportModelVisitor visitor )
        {
            _visitor = visitor ?? throw new ArgumentNullException();
        }

        public TDataBase Convert ( TImport obj )
        {
            return ( TDataBase )obj.Convert( _visitor );
        }
    }
}
