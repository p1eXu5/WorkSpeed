using System;
using System.Collections.Generic;
using Agbm.NpoiExcel;
using WorkSpeed.Business.FileModels;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels.Converters
{
    public class ImportModelConverter< TImport, TDataBase > : ITypeConverter< TImport, TDataBase >
        where   TImport : ImportModel
        where TDataBase : new()
    {
        private static readonly Dictionary< Type, Type > _typeMap;

        private readonly IImportModelVisitor _visitor;

        static ImportModelConverter ()
        {
            _typeMap[ typeof( ProductImportModel ) ] = typeof( Product );
            _typeMap[ typeof( EmployeeImportModel ) ] = typeof( Employee );
        }

        public ImportModelConverter ()
        {
            _visitor = new ImportModelVisitor();
        }

        public ImportModelConverter ( IImportModelVisitor visitor )
        {
            _visitor = visitor ?? throw new ArgumentNullException();
        }

        public TDataBase Convert ( TImport obj )
        {
            return ( TDataBase )(obj.Convert( _visitor ));
        }
    }
}
