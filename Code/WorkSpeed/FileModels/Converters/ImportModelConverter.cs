using System;
using System.Collections.Generic;
using Agbm.NpoiExcel;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels.Converters
{
    public class ImportModelConverter< TImport, TDataBase > : ITypeConverter< TImport, TDataBase >
        where   TImport : ImportModel
        where TDataBase : new()
    {
        private static readonly Dictionary< Type, Type > _typeMap;

        private readonly IImportModelVisitor _visitor;

        static ImportModelConverter ()
        {
            _typeMap[ typeof(Empl) ] = typeof();
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
