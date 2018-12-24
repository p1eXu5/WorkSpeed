using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        /// <param name="progressFunc"></param>
        /// <returns></returns>
        public static IEnumerable< TOutType > GetEnumerable< TIn, TOutType>( SheetTable sheetTable,
                                                                             Dictionary< string, (string header, int column) > propertyMap,  
                                                                             ITypeConverter< TIn, TOutType > typeConverter )
        {
            var typeCollection = FillModelCollection( sheetTable, typeof( TIn ), propertyMap );
            var typedCollection = typeCollection.Cast< TIn >().Select( typeConverter.Convert ).ToArray();

            return typedCollection;
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

        [ SuppressMessage( "ReSharper", "PossibleNullReferenceException" ) ]
        private static ICollection FillModelCollection( SheetTable sheetTable,  
                                                        Type type,  
                                                        Dictionary< string, (string header, int column) > headersMap )
        {
            CheckType( type );
            object[] typeInstanceCollection = new object[ sheetTable.RowCount ];

            double progress = 0.0;
            double percent = 1.0 / sheetTable.RowCount;
            OnProgressChanged( progress );

            Parallel.For( 0, sheetTable.RowCount, j =>
            {

                object typeInstance = Activator.CreateInstance(type);

                //Debug.WriteLine( sw.Elapsed ); 0000196

                foreach (var propertyName in headersMap.Keys) {

                    var cell = sheetTable[ j, headersMap[ propertyName ].column ];

                    Type propertyType = type.GetProperty( propertyName ).PropertyType;

                    if ( propertyType == typeof( int ) ) {
                        type.GetProperty( propertyName ).SetValue( typeInstance, (int)cell );
                    }
                    else if ( propertyType == typeof( double ) ) {
                        type.GetProperty( propertyName ).SetValue( typeInstance, ( double )cell );
                    }
                    else if ( propertyType == typeof( string ) ) {
                        type.GetProperty( propertyName ).SetValue( typeInstance, ( string )cell );
                    }
                    else if ( propertyType == typeof( bool ) ) {
                        type.GetProperty( propertyName ).SetValue( typeInstance, ( bool )cell );
                    }
                    else if ( propertyType == typeof( DateTime ) ) {
                        type.GetProperty( propertyName ).SetValue( typeInstance, ( DateTime )cell );
                    }
                }

                typeInstanceCollection[ j ] = typeInstance;

                OnProgressChanged( percent );
            } );

            return typeInstanceCollection;
        }

        public static event EventHandler< ProgressChangedEventArgs > ProgressChangedEvent;

        private static void OnProgressChanged ( double progress )
        {
            ProgressChangedEvent?.Invoke( null, new ProgressChangedEventArgs( progress ) );
        }
    }

    public class ProgressChangedEventArgs : EventArgs
    {
        public ProgressChangedEventArgs ( double progress )
        {
            Progress = progress;
        }

        public double Progress { get; }
    }
}
