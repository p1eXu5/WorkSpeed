using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using NpoiExcel.Attributes;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Helpers.StringExtensions;
using static Helpers.EasyTypeBuilder;

namespace NpoiExcel
{
    public static class ExcelImporter
    {
        #region ImportData

        public static SheetTable ImportData (string fileName, int sheetIndex)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {

                return ImportData(stream, sheetIndex);
            }
        }

        public static SheetTable ImportData (Stream stream, int sheetIndex)
        {
            if (sheetIndex < 0) throw new ArgumentException("sheetIndex must be equal or greater than zero.", nameof(sheetIndex));

            // Load sheetTable:
            ISheet sheet;

            try {
                sheet = GetSheet(stream, sheetIndex);
            }
            catch {
                throw new FileFormatException($"{stream} has invalid data format or has no sheetTable with {sheetIndex} index.");
            }

            return new SheetTable(sheet);
        }


        public static ICollection ImportData (string fileName, Type type, int sheetIndex)
        {
            using (var stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                return ImportData (stream, type, sheetIndex);
            }
        }

        public static ICollection ImportData (Stream source, Type type, int sheetIndex)
        {
            SheetTable sheetTable;

            try {
                sheetTable = ImportData (source, sheetIndex);
            }
            catch (ArgumentException) {
                return GetEmptyCollection (type);
            }

            var typeRepository = new TypeRepository();
            typeRepository.RegisterType( type, typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            var typeWithMap = typeRepository.GetTypeWithMap( sheetTable );

            if (null == typeWithMap.type) {
                return null;
            }

            return FillModelCollection(sheetTable, type, typeWithMap.propertyMap);
        }

        #endregion

        /// <summary>
        /// Returns enumerable collection of TOutType.
        /// </summary>
        /// <typeparam name="TIn">Converter origin type</typeparam>
        /// <typeparam name="TOutType">Converter output type</typeparam>
        /// <param name="sheetTable"><see cref="SheetTable"/></param>
        /// <param name="propertyMap">Dictionary&lt; propertyName, header &gt;</param>
        /// <param name="typeConverter">Type typeConverter</param>
        /// <returns></returns>
        public static IEnumerable< TOutType > GetEnumerable< TIn, TOutType>( SheetTable sheetTable,
                                                                             Dictionary< string, (string header, int column) > propertyMap,  
                                                                             ITypeConverter< TIn, TOutType > typeConverter )
        {
            var typeCollection = FillModelCollection( sheetTable, typeof( TIn ), propertyMap );
            return typeCollection.Cast< TIn >().Select( obj => ( TOutType )typeConverter.Convert( obj ) );
        }


        /// <summary>
        /// Checks out is a type has a parameterless constructor?
        /// </summary>
        /// <param name="type"></param>
        private static void CheckType (Type type)
        {
            if (type.GetConstructors().Count(c => c.IsPublic && c.GetParameters().Length == 0) == 0) {
                throw new TypeAccessException($"{type} has no public parameterless constructor!");
            }
        }


        private static ISheet GetSheet (Stream stream, int sheetIndex)
        {
            IWorkbook book;

            try { 
                book = new XSSFWorkbook(stream);
            }
            catch(Exception ex) {

                Debug.WriteLine (ex.Message);
                book = new HSSFWorkbook(stream);
            }

            return book.GetSheetAt(sheetIndex);
        }

        private static ICollection FillModelCollection( SheetTable sheetTable,  Type type,  Dictionary< string, (string header, int column) > headersMap )
        {
            CheckType( type );
            ArrayList typeInstanceCollection = new ArrayList(sheetTable.RowCount);

            for (var j = 0; j < sheetTable.RowCount; ++j) {

                object typeInstance = Activator.CreateInstance(type);

                foreach (var propertyName in headersMap.Keys) {

                    var cell = sheetTable[ j, headersMap[ propertyName ].column ];

                    // ReSharper disable once PossibleNullReferenceException
                    type.GetProperty( propertyName ).SetValue( typeInstance, cell );
                }

                typeInstanceCollection.Add(typeInstance);
            }

            return typeInstanceCollection;
        }
    }
}
