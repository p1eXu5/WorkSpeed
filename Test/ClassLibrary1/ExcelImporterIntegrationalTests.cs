using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Agbm.Helpers.Extensions;
using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using NUnit.Framework;

namespace ClassLibrary1
{
    [TestFixture]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Local" ) ]
    public class ExcelImporterIntegrationalTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }

        [ Test ]
        public void ImportData_XlsxFile_CanRead ()
        {
            var file = "test.xlsx".AppendAssemblyPath( "TestFiles" );

            var res = ExcelImporter.ImportData( file, typeof( ImportedFileClass ) ).Cast< ImportedFileClass >();

            Assert.That( res, Is.Not.Empty );
        }

        [ Test ]
        public void ImportData_XlsFile_CanRead ()
        {
            var file = "test.xls".AppendAssemblyPath( "TestFiles" );

            var res = ExcelImporter.ImportData( file, typeof( ImportedFileClass ) ).Cast< ImportedFileClass >();

            Assert.That( res, Is.Not.Empty );
        }




        #region Factory

        class ImportedFileClass
        {
            [Header( "Код товара" )]        public int Id { get; set; }
            [Header( "Номенклатура" )]      public string Name { get; set; }

            [Header( "Колво в коробке" )]   public int? CartonQuantity { get; set; }

            [Header( "Вес ед" )]            public double? ItemWeight { get; set; }

            [Header( "Длина коробки" )]
            [Header( "ДлинаКоробки_см" )]
                                            public double? CartonLength { get; set; }

            [Header( "Ширина коробки" )]
            [Header( "ШиринаКоробки_см" )]
                                            public double? CartonWidth { get; set; }

            [Header( "Высота коробки" )]
            [Header( "ВысотаКоробки_см" )]
                                            public double? CartonHeight { get; set; }

            [Header( "ДлинаЕд_см" )]         public double? ItemLength { get; set; }
            [Header( "ШиринаЕд_см" )]         public double? ItemWidth { get; set; }
            [Header( "ВысотаЕд_см" )]         public double? ItemHeight { get; set; }
        }

        #endregion
    }
}
