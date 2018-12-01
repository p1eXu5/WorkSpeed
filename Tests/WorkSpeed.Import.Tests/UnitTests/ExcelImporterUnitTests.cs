﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Moq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;
using WorkSpeed.Import.Attributes;
using static WorkSpeed.Import.Tests.TestHelper;

namespace WorkSpeed.Import.Tests.UnitTests
{
    [TestFixture]
    [SuppressMessage ("ReSharper", "PossibleNullReferenceException")]
    [SuppressMessage ("ReSharper", "MemberCanBePrivate.Local")]
    [SuppressMessage ("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    [SuppressMessage ("ReSharper", "UnusedMember.Global")]
    [SuppressMessage ("ReSharper", "UnusedMember.Local")]
    public class ExcelImporterUnitTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-Us", false);
        }

        #region ImportData

        [Test]
        public void GetSheetAtZero_FileDoesNotExist_Throws()
        {
            // Arrange:
            var file = "not_existed_file.xls";

            // Action:
            var ex = Assert.Catch<ArgumentException> (() => ExcelImporter.ImportData (file, _stubTypeRepository.Object));

            // Assert:
            StringAssert.Contains ("File doesn't exist", ex.Message);
        }

        [TestCase ("test.tst")]
        [TestCase ("test.xls")]
        [TestCase ("test.xlsx")]
        public void ImportData_FileIsNotExcelFileOrFileEmpty_ReturnsEmptyCollection(string fileName)
        {
            // Arrange:
            var file = CreateFakeEmptyTestFile(fileName);

            // Action:
            var result = ExcelImporter.ImportData(file, _stubTypeRepository.Object);

            // Assert:
            Assert.That(0 == result?.Count());

            // Cleanup:
            File.Delete (file);
        }

        [Test]
        public void ImportData_ValidExcelFileHasNoSheets_ReturnsEmptyCollection()
        {
            // Arrange:
            var file = CreateFakeZeroLengthXlsxFile();

            // Action:
            var result = ExcelImporter.ImportData (file, _stubTypeRepository.Object);

            // Assert:
            Assert.That (0 == result?.Count());
        }

        [Test]
        public void ImportData_TypeRepositoryIsNull_Throws()
        {
            // Arrange:
            var file = CreateTestXlsxFileWithSingleStringCell();
            ITypeRepository repo = null;

            // Action:
            var ex = Assert.Catch<ArgumentNullException> (() => ExcelImporter.ImportData (file, repo));

            // Assert:
            StringAssert.Contains ("typeRepository can not be null", ex.Message);
        }

        [Test]
        public void ImportData_XlsxFileContainsRecordsAreCorrespondedTypeInTypeRepository_ReturnsImportModelCollection()
        { }

        #endregion


        #region String Property

        [TestCase ("", "string")]
        [TestCase (" ", "string")]
        [TestCase ("value", "string")]
        [TestCase ("1", "string")]
        [TestCase ("1.02", "string")]
        [TestCase (",#2-", "string")]
        [TestCase ("Да", "string")]
        public void ImportDataFromExcel_StringPropertyStringCell_CanReadStringCell(string cellValue, string propertyType)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile (cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That (cellValue == (element?.GetType().GetProperties()[0].GetValue (element)?.ToString() ?? ""));
        }

        [TestCase (0, "string", "0")]
        [TestCase (1, "string", "1")]
        [TestCase (-1, "string", "-1")]
        [TestCase (1.00123, "string", "1.00123")]
        [TestCase (-1.00123, "string", "-1.00123")]
        [TestCase(Double.MaxValue, "string", "0.0")]
        [TestCase(Double.MinValue, "string", "0.0")]
        public void ImportDataFromExcel_StringPropertyNumericCell_CanReadStringCell(double cellValue, string propertyType, string propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile (cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That (propertyValue == element.GetType().GetProperties()[0].GetValue (element).ToString());
        }

        [TestCase(true, "string", "Да")]
        [TestCase(false, "string", "Нет")]
        public void ImportDataFromExcel_StringPropertyBooleanCell_CanReadStringCell(bool cellValue, string propertyType, string propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That(propertyValue == element.GetType().GetProperties()[0].GetValue(element).ToString());
        }

        #endregion


        #region Int Property

        [TestCase ("", "int", 0)]
        [TestCase (" ", "int", 0)]
        [TestCase ("1", "int", 1)]
        [TestCase ("1.02", "int", 1)]
        [TestCase ("1,02", "int", 1)]
        [TestCase ("-1", "int", -1)]
        [TestCase ("-1.00123", "int", -1)]
        [TestCase ("-1,00123", "int", -1)]
        [TestCase ("Да", "int", 1)]
        [TestCase ("Yes", "int", 1)]
        [TestCase ("Нет", "int", 0)]
        [TestCase ("No", "int", 0)]
        [TestCase ("1a", "int", 0)]
        [TestCase ("d1.02", "int", 0)]
        [TestCase ("_-1", "int", 0)]
        [TestCase ("-1-", "int", 0)]
        [TestCase ("-1.,00123", "int", 0)]
        [TestCase ("Даc", "int", 0)]
        [TestCase ("Нетc", "int", 0)]
        [TestCase ("Yesc", "int", 0)]
        [TestCase ("Noc", "int", 0)]
        public void ImportDataFromExcel_IntPropertyStringCell_CanReadIntAndBoolCell(string cellValue, string propertyType, int propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile (cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That (propertyValue == (int)(element.GetType().GetProperties()[0].GetValue (element)));
        }


        [TestCase(0, "int", 0)]
        [TestCase(999999, "int", 999999)]
        [TestCase(-98765, "int", -98765)]
        [TestCase(Int32.MaxValue, "int", Int32.MaxValue)]
        [TestCase(Int32.MinValue, "int", Int32.MinValue)]
        public void ImportDataFromExcel_IntPropertyIntCell_CanReadIntAndBoolCell(int cellValue, string propertyType, int propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That(propertyValue == (int)(element.GetType().GetProperties()[0].GetValue(element)));
        }


        [TestCase(true, "int", 1)]
        [TestCase(false, "int", 0)]
        public void ImportDataFromExcel_IntPropertyBooleanCell_CanReadIntAndBoolCell(bool cellValue, string propertyType, int propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That(propertyValue == (int)(element.GetType().GetProperties()[0].GetValue(element)));
        }

        #endregion


        #region Double Property

        [TestCase ("", "double", 0.0)]
        [TestCase (" ", "double", 0.0)]
        [TestCase ("1", "double", 1.0)]
        [TestCase ("1.02", "double", 1.02)]
        [TestCase ("1,02", "double", 1.02)]
        [TestCase ("-1", "double", -1.0)]
        [TestCase ("-1.00123", "double", -1.00123)]
        [TestCase ("-1,00123", "double", -1.00123)]
        [TestCase ("Да", "double", 1.0)]
        [TestCase ("Yes", "double", 1.0)]
        [TestCase ("Нет", "double", 0.0)]
        [TestCase ("No", "double", 0.0)]
        [TestCase ("1a", "double", 0.0)]
        [TestCase ("d1.02", "double", 0.0)]
        [TestCase ("_-1", "double", 0.0)]
        [TestCase ("-1-", "double", 0.0)]
        [TestCase ("-1.,00123", "double", 0.0)]
        [TestCase ("Даc", "double", 0.0)]
        [TestCase ("Нетc", "double", 0.0)]
        [TestCase ("Yesc", "double", 0.0)]
        [TestCase ("Noc", "double", 0.0)]
        public void ImportDataFromExcel_DoublePropertyStringCell_CanReadDoubleAndBoolCell(string cellValue, string propertyType, double propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile (cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;
            
            Assert.That (propertyValue, Is.EqualTo ((double)(element.GetType().GetProperties()[0].GetValue (element))));
        }


        [TestCase(0.0, "double", 0.0)]
        [TestCase(1.02345, "double", 1.02345)]
        [TestCase(-1.02345, "double", -1.02345)]
        [TestCase(Single.MaxValue, "double", Single.MaxValue)]
        [TestCase(Single.MinValue, "double", Single.MinValue)]
        [TestCase(Double.MaxValue, "double", 0.0)]
        [TestCase(Double.MinValue, "double", 0.0)]
        public void ImportDataFromExcel_DoublePropertyDoubleCell_CanReadDoubleAndBoolCell(double cellValue, string propertyType, double propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            var delta = Math.Log10 (Math.Abs(propertyValue)) > 25 ? 1e+25 : 1e13; 
            Assert.That(propertyValue, Is.EqualTo((double)(element.GetType().GetProperties()[0].GetValue(element))).Within (delta));
        }


        [TestCase(true, "double", 1.0)]
        [TestCase(false, "double", 0.0)]
        public void ImportDataFromExcel_DoublePropertyBooleanCell_CanReadDoubleAndBoolCell(bool cellValue, string propertyType, double propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            var delta = Math.Log10(Math.Abs(propertyValue)) > 25 ? 1e+25 : 1e13;
            Assert.That(propertyValue, Is.EqualTo((double)(element.GetType().GetProperties()[0].GetValue(element))).Within(delta));
        }

        #endregion


        #region Boolean Property

        [TestCase ("", "bool", false)]
        [TestCase (" ", "bool", false)]
        [TestCase ("Да", "bool", true)]
        [TestCase ("дА", "bool", true)]
        [TestCase ("Yes", "bool", true)]
        [TestCase ("yEs", "bool", true)]
        [TestCase ("Нет", "bool", false)]
        [TestCase ("НеТ", "bool", false)]
        [TestCase ("NO", "bool", false)]
        [TestCase ("1", "bool", false)]
        [TestCase ("-1", "bool", false)]
        [TestCase ("1a", "bool", false)]
        [TestCase ("d1.02", "bool", false)]
        [TestCase ("_-1", "bool", false)]
        [TestCase ("-1-", "bool", false)]
        [TestCase ("-1.,00123", "bool", false)]
        [TestCase ("0.00123", "bool", false)]
        [TestCase ("1,00123", "bool", false)]
        [TestCase ("-1.00123", "bool", false)]
        [TestCase ("-0,00123", "bool", false)]
        [TestCase ("Даc", "bool", false)]
        [TestCase ("Нетc", "bool", false)]
        [TestCase ("Yesc", "bool", false)]
        [TestCase ("Noc", "bool", false)]
        public void ImportDataFromExcel_BoolPropertyStringCell_CanReadBoolCell(string cellValue, string propertyType, bool propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile (cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;
            
            Assert.That (propertyValue, Is.EqualTo ((bool)(element.GetType().GetProperties()[0].GetValue (element))));
        }


        [TestCase(0.0, "bool", false)]
        [TestCase(1.02345, "bool", true)]
        [TestCase(-1.02345, "bool", true)]
        [TestCase(Single.MaxValue, "bool", true)]
        [TestCase(Single.MinValue, "bool", true)]
        [TestCase(Double.MaxValue, "bool", false)]
        [TestCase(Double.MinValue, "bool", false)]
        public void ImportDataFromExcel_BoolPropertyNumericCell_CanReadBoolCell(double cellValue, string propertyType, bool propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That(propertyValue, Is.EqualTo((bool)(element.GetType().GetProperties()[0].GetValue(element))));
        }


        [TestCase(true, "bool", true)]
        [TestCase(false, "bool", false)]
        public void ImportDataFromExcel_BoolPropertyBoolCell_CanReadBoolCell(bool cellValue, string propertyType, bool propertyValue)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportData (CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That(propertyValue, Is.EqualTo((bool)(element.GetType().GetProperties()[0].GetValue(element))));
        }

        #endregion



        [TearDown]
        public void Cleanup()
        {
            if (File.Exists (_fakeXlsxFileName.GetFullPath())) {
                File.Delete (_fakeXlsxFileName.GetFullPath());
            }

            _memoryStream?.Close();
        }

        #region Factory

        private readonly Mock<ITypeRepository> _stubTypeRepository = new Mock<ITypeRepository>();

        private ITypeRepository GetTypeRepository()
        {
            return new TypeRepository();
        }

        #endregion

        #region Stream Factory

        private const string _fakeXlsxFileName = "fakefile.xlsx";
        private const string _testHeaderName = "Test Header";
        private const string _mainCellText = "Main Cell";
        private MemoryStream _memoryStream;

        /// <summary>
        /// Creates file with _fakeXlsxFileName name.
        /// File is deleted after each test in [TestDown]Cleanup() method.
        /// </summary>
        /// <returns></returns>
        private string CreateFakeZeroLengthXlsxFile()
        {
            using (var stream = new FileStream (_fakeXlsxFileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                return stream.Name;
            }
        }

        /// <summary>
        /// Create _fakeXlsxFileName named file with one cell which contains
        /// string with _mainCellText value.
        /// File is deleted after each test in [TestDown]Cleanup() method.
        /// </summary>
        /// <returns></returns>
        private string CreateTestXlsxFileWithSingleStringCell()
        {
            IWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();

            sheet.CreateRow (10).CreateCell (10).SetCellValue (_mainCellText);

            using (var stream = new FileStream (_fakeXlsxFileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        private string CreateTestFile (TableRect columns, float density, byte mainTop, byte mainLeft)
        {
            Random rnd = new Random((int)DateTime.Now.TimeOfDay.TotalSeconds);

            IWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();

            var nNc = columns.Right - columns.Left;
            var nc = density > 1 
                        ? (density - Math.Floor (density)) * nNc 
                        : density * nNc;

            for (int j = columns.Top; j <= columns.Bottom; j++) {

                var row = sheet.CreateRow (j);

                for (int i = 0; i < nc; i++) {

                    int nextCell = columns.Left + rnd.Next (nNc);

                    while (!(null == row.GetCell (nextCell))) {
                        ++nextCell;
                        if (nextCell == columns.Right) {
                            nextCell = columns.Left;
                        }
                    }

                    row.CreateCell (nextCell).SetCellValue ("value");
                }
            }

            if (sheet.GetRow (mainTop)?.GetCell (mainLeft) == null) {
                sheet.CreateRow (mainTop).CreateCell (mainLeft).SetCellValue (_mainCellText);
            }
            else {
                sheet.GetRow (mainTop).GetCell (mainLeft).SetCellValue (_mainCellText);
            }

            using (var stream = new FileStream (_fakeXlsxFileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        private string CreateTestHeadedFile (string testValue)
        {
            IWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();

            sheet.CreateRow (9).CreateCell (10).SetCellValue (_testHeaderName);
            sheet.CreateRow (10).CreateCell (10).SetCellValue (testValue);

            using (var stream = new FileStream (_fakeXlsxFileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        private string CreateTestHeadedFile (double testValue)
        {
            IWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();

            sheet.CreateRow (9).CreateCell (10).SetCellValue (_testHeaderName);
            sheet.CreateRow (10).CreateCell (10).SetCellValue (testValue);

            using (var stream = new FileStream (_fakeXlsxFileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        private string CreateTestHeadedFile (bool testValue)
        {
            IWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();

            sheet.CreateRow (9).CreateCell (10).SetCellValue (_testHeaderName);
            sheet.CreateRow (10).CreateCell (10).SetCellValue (testValue);

            using (var stream = new FileStream (_fakeXlsxFileName.GetFullPath(), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        private MemoryStream SetMemoryStream (params string[] headers)
        {
            XSSFWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet ("TestSheet");
            IRow rowH = sheet.CreateRow (2);
            IRow rowV = sheet.CreateRow (3);

            int i = 0;

            foreach (var header in headers) {
                rowH.CreateCell (i).SetCellValue (header);
                rowV.CreateCell (i++).SetCellValue ($"value: {header}");
            }

            _memoryStream = new MemoryStream();
            book.Write (_memoryStream, true);
            _memoryStream.Position = 0;

            return _memoryStream;
        }


        #endregion

        #region Type Builder

        /// <summary>
        /// Creates Type with one headered property.
        /// </summary>
        /// <param name="propertyTypeName">Name of propert type.</param>
        /// <returns><see cref="Type"/></returns>
        private Type GetModelType (string propertyTypeName)
        {
            var typeBuilder = GetTypeBuilder();
            CreateProperty(typeBuilder, propertyTypeName, $"TestProperty");

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates Type with propertyes that correspondes passed properties.
        /// </summary>
        /// <param name="headers">Header names.</param>
        /// <returns><see cref="Type"/></returns>
        private Type GetModelType (params string[] headers)
        {
            var typeBuilder = GetTypeBuilder();

            foreach (var header in headers) {
                CreateProperty (typeBuilder, "string", $"Property{header.RemoveWhitespaces()}", header);
            }

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Creates Type with propertyes that correspondes passed property types and their properties.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private Type GetModelType(params (string header, string proprtyType)[] properties)
        {
            var typeBuilder = GetTypeBuilder();

            foreach (var tuple in properties) {
                CreateProperty(typeBuilder, tuple.proprtyType, $"Property{tuple.header.RemoveWhitespaces()}", tuple.header);
            }

            return typeBuilder.CreateType();
        }

        private static TypeBuilder GetTypeBuilder()
        {
            AppDomain domain = AppDomain.CurrentDomain;

            AssemblyName assemblyName = new AssemblyName ("TestAssembly");
            AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly (assemblyName, AssemblyBuilderAccess.Run);

            ModuleBuilder module = assemblyBuilder.DefineDynamicModule ("TestModul");

            return module.DefineType ("TestType", TypeAttributes.Public);
        }

        [SuppressMessage ("ReSharper", "AssignNullToNotNullAttribute")]
        private static void CreateProperty (TypeBuilder typeBuilder, string propertyTypeName, string propertyName, string header = null)
        {
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            
            var fieldName = "_" + propertyName.Substring (0, 1).ToLowerInvariant() + propertyName.Substring (1);

            Type propertyType;

            switch (propertyTypeName) {

                    case "int":
                        propertyType = typeof(int);
                        break;

                    case "double":
                        propertyType = typeof(double);
                        break;

                    case "bool":
                        propertyType = typeof(bool);
                        break;

                    case "string":
                    default:
                        propertyType = typeof(string);
                        break;
            }


            FieldBuilder fieldBuilder = typeBuilder.DefineField (fieldName, propertyType, FieldAttributes.Private);
            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty (propertyName, PropertyAttributes.HasDefault, propertyType, null);

            MethodBuilder getterBuilder = typeBuilder.DefineMethod ($"get_{propertyName}", getSetAttr, propertyType, Type.EmptyTypes);
            LoadIlToGetter (getterBuilder, fieldBuilder);

            MethodBuilder setterBuilder = typeBuilder.DefineMethod ($"set_{propertyName}", getSetAttr, null, new[] { propertyType });
            LoadIlToSetter (setterBuilder, fieldBuilder);

            propertyBuilder.SetGetMethod (getterBuilder);
            propertyBuilder.SetSetMethod (setterBuilder);

            var attrBuilder = GetHeaderAttributeBuilder (header ?? _testHeaderName);
            propertyBuilder.SetCustomAttribute (attrBuilder);


            #region Local Functions

            void LoadIlToGetter (MethodBuilder getterBldr, FieldBuilder fieldBldr)
            {
                ILGenerator getterGenerator = getterBldr.GetILGenerator();
                getterGenerator.Emit (OpCodes.Ldarg_0);
                getterGenerator.Emit (OpCodes.Ldfld, fieldBldr);
                getterGenerator.Emit (OpCodes.Ret);
            }

            void LoadIlToSetter (MethodBuilder setterBldr, FieldBuilder fieldBldr)
            {
                ILGenerator setterGenerator = setterBldr.GetILGenerator();
                setterGenerator.Emit (OpCodes.Ldarg_0);
                setterGenerator.Emit (OpCodes.Ldarg_1);
                setterGenerator.Emit (OpCodes.Stfld, fieldBldr);
                setterGenerator.Emit (OpCodes.Ret);
            }

            CustomAttributeBuilder GetHeaderAttributeBuilder(object headerName)
            {
                ConstructorInfo ci = typeof(HeaderAttribute).GetConstructor (new[]{ typeof(string) });
                return new CustomAttributeBuilder (ci, new[] { headerName });
            }

            #endregion
        }

        #endregion

    }
}
