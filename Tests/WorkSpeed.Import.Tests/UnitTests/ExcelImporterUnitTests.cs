using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
        #region GetFirstCell

        [Test]
        public void ImportDataFromExcel_FileDoesNotExist_Throw()
        {
            // Action:
            Assert.That(() => ExcelImporter.ImportDataFromExcel(GetFullPath("doesnotexistfile.xlsx"), typeof(FakeHeadlessModelClass)), Throws.Exception);
        }

        [Test]
        public void ImportDataFromExcel_XlsxFileDoesNotContainData_ReturnsEmptyCollection()
        {
            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(GetFullPath ("empty.xlsx"), typeof(FakeHeadlessModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel_ZeroLenghtFileWithCorrectExtension_ReturnsEmptyCollection()
        {
            // Arrange:
            var fileName = CreateFakeZeroLengthXlsxFile();

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeHeadlessModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel_OneColumnOneProperty_ReturnsTheMostTopLeftCell ()
        {
            // Arrange:
            var fileName = CreateTestFileWithSingleStringCell ();

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeHeadlessModelClass));

            // Assert:
            Assert.That (_mainCellText == resColl.OfType<FakeHeadlessModelClass>().First().MainCell);
        }

        [Test]
        public void ImportDataFromExcel_MultiColumnOneProperty_ReturnsEmptyCollection ()
        {
            // Arrange:
            var fileName = CreateTestFile (new TableRect(5, 5, 5), (float)1.0, 5, 5);

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeHeadlessModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel__ModelPropertyAttributeless_PropertyNameEqualsCellHeaderWithoutWhiteSpaces_CellFilled__ReturnsCellValue()
        {
            // Arrange:
            var cellValue = "Test Value";

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), typeof(FakeModelClassWithTestHeaderNamedProperty));

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That (resColl.Count != 0);
            Assert.That(cellValue == (element?.GetType().GetProperties()[0].GetValue(element)?.ToString() ?? ""));
        }

        [Test]
        public void ImportDataFromExcel_ModelPropertyWithAttributeNotCorrespondingCellHeader_ReturnsEmptyCollection()
        {
            // Arrange:
            var cellValue = "Test Value";

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), typeof(FakeModelClassWithWrongHeaderAttribute));

            // Assert:
            Assert.That(resColl.Count == 0);
        }

        [Test]
        public void ImportDataFromExcel_ModelPropertyWithoutParameterlessConstructor_ReturnsEmptyCollection()
        {
            // Arrange:
            var cellValue = "Test Value";

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), typeof(FakeModelClassWithoutParameterlessConstructor));

            // Assert:
            Assert.That(resColl.Count == 0);
        }

        [Test]
        public void ImportDataFromExcel_HeaderedXlsFileWithOneColumn_ReturnsCollectionWithCellValue()
        {
            // Action
            var resColl = ExcelImporter.ImportDataFromExcel (GetFullPath ("testExcel2003FileWithSingleHeadedCell.xls"), GetModelType (new[] { "string" }));

            // Assert
            Assert.That (1 == resColl.Count);
        }


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
            var resColl = ExcelImporter.ImportDataFromExcel (CreateTestHeadedFile (cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel (CreateTestHeadedFile (cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel (CreateTestHeadedFile (cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel (CreateTestHeadedFile (cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel (CreateTestHeadedFile (cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

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
            var resColl = ExcelImporter.ImportDataFromExcel(CreateTestHeadedFile(cellValue), modelType);

            // Assert:
            var enumerator = resColl.GetEnumerator();
            var element = enumerator.MoveNext() ? enumerator.Current : null;

            Assert.That(propertyValue, Is.EqualTo((bool)(element.GetType().GetProperties()[0].GetValue(element))));
        }

        #endregion

        #endregion


        [TearDown]
        public void Cleanup()
        {
            RemoveFakeFile();
        }


        #region Factory

        private const string _fakeXlsxFileName = "fakefile.xlsx";
        private const string _mainCellText = "Main Cell";
        private const string _testHeaderName = "Test Header";

        private Type GetModelType (params string[] propertyTypes)
        {
            AppDomain domain = AppDomain.CurrentDomain;
            AssemblyName assemblyName = new AssemblyName("TestAssembly");
            AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assemblyBuilder.DefineDynamicModule("TestModul");

            TypeBuilder typeBuilder = module.DefineType ("TestType", TypeAttributes.Public);

            var i = 0;
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            foreach (var propertyType in propertyTypes) {

                switch (propertyType) {
                        
                    case "string" :

                        FieldBuilder fieldBuilder = typeBuilder.DefineField ($"_testProperty{i}", typeof(string), FieldAttributes.Private);
                        PropertyBuilder propertyBuilder = typeBuilder.DefineProperty ($"TestProperty{i}", PropertyAttributes.HasDefault, typeof(string), null);

                        MethodBuilder getterBuilder = typeBuilder.DefineMethod ($"get_TestProperty{i}", getSetAttr, typeof(string), Type.EmptyTypes);
                        LoadIlToGetter (getterBuilder, fieldBuilder);

                        MethodBuilder setterBuilder = typeBuilder.DefineMethod ($"set_TestProperty{i}", getSetAttr, null, new[] {typeof(string)});
                        LoadIlToSetter (setterBuilder, fieldBuilder);

                        propertyBuilder.SetGetMethod (getterBuilder);
                        propertyBuilder.SetSetMethod (setterBuilder);

                        var attrBuilder = GetHeaderAttributeBuilder(_testHeaderName);
                        propertyBuilder.SetCustomAttribute (attrBuilder);

                        break;

                    case "int" :

                        fieldBuilder = typeBuilder.DefineField ($"_testProperty{i}", typeof(int), FieldAttributes.Private);
                        propertyBuilder = typeBuilder.DefineProperty ($"TestProperty{i}", PropertyAttributes.HasDefault, typeof(int), null);

                        getterBuilder = typeBuilder.DefineMethod ($"get_TestProperty{i}", getSetAttr, typeof(int), Type.EmptyTypes);
                        LoadIlToGetter (getterBuilder, fieldBuilder);

                        setterBuilder = typeBuilder.DefineMethod ($"set_TestProperty{i}", getSetAttr, null, new[] {typeof(int)});
                        LoadIlToSetter (setterBuilder, fieldBuilder);

                        propertyBuilder.SetGetMethod (getterBuilder);
                        propertyBuilder.SetSetMethod (setterBuilder);

                        attrBuilder = GetHeaderAttributeBuilder(_testHeaderName);
                        propertyBuilder.SetCustomAttribute (attrBuilder);

                        break;

                    case "double" :

                        fieldBuilder = typeBuilder.DefineField ($"_testProperty{i}", typeof(double), FieldAttributes.Private);
                        propertyBuilder = typeBuilder.DefineProperty ($"TestProperty{i}", PropertyAttributes.HasDefault, typeof(double), null);

                        getterBuilder = typeBuilder.DefineMethod ($"get_TestProperty{i}", getSetAttr, typeof(double), Type.EmptyTypes);
                        LoadIlToGetter (getterBuilder, fieldBuilder);

                        setterBuilder = typeBuilder.DefineMethod ($"set_TestProperty{i}", getSetAttr, null, new[] {typeof(double)});
                        LoadIlToSetter (setterBuilder, fieldBuilder);

                        propertyBuilder.SetGetMethod (getterBuilder);
                        propertyBuilder.SetSetMethod (setterBuilder);

                        attrBuilder = GetHeaderAttributeBuilder(_testHeaderName);
                        propertyBuilder.SetCustomAttribute (attrBuilder);

                        break;

                    case "bool" :

                        fieldBuilder = typeBuilder.DefineField ($"_testProperty{i}", typeof(bool), FieldAttributes.Private);
                        propertyBuilder = typeBuilder.DefineProperty ($"TestProperty{i}", PropertyAttributes.HasDefault, typeof(bool), null);

                        getterBuilder = typeBuilder.DefineMethod ($"get_TestProperty{i}", getSetAttr, typeof(bool), Type.EmptyTypes);
                        LoadIlToGetter (getterBuilder, fieldBuilder);

                        setterBuilder = typeBuilder.DefineMethod ($"set_TestProperty{i}", getSetAttr, null, new[] {typeof(bool)});
                        LoadIlToSetter (setterBuilder, fieldBuilder);

                        propertyBuilder.SetGetMethod (getterBuilder);
                        propertyBuilder.SetSetMethod (setterBuilder);

                        attrBuilder = GetHeaderAttributeBuilder(_testHeaderName);
                        propertyBuilder.SetCustomAttribute (attrBuilder);

                        break;
                }

                ++i;
            }

            return typeBuilder.CreateType();


            void LoadIlToGetter (MethodBuilder getterBuilder, FieldBuilder fieldBuilder)
            {
                ILGenerator getterGenerator = getterBuilder.GetILGenerator();
                getterGenerator.Emit (OpCodes.Ldarg_0);
                getterGenerator.Emit (OpCodes.Ldfld, fieldBuilder);
                getterGenerator.Emit (OpCodes.Ret);
            }

            void LoadIlToSetter(MethodBuilder setterBuilder, FieldBuilder fieldBuilder)
            {
                ILGenerator setterGenerator = setterBuilder.GetILGenerator();
                setterGenerator.Emit (OpCodes.Ldarg_0);
                setterGenerator.Emit (OpCodes.Ldarg_1);
                setterGenerator.Emit (OpCodes.Stfld, fieldBuilder);
                setterGenerator.Emit (OpCodes.Ret);
            }
        }

        private static CustomAttributeBuilder GetHeaderAttributeBuilder(object headerObj)
        {
            ConstructorInfo ci = typeof(HeaderAttribute).GetConstructor (new[]{ typeof(string) });
            // ReSharper disable once AssignNullToNotNullAttribute
            CustomAttributeBuilder attrBuilder = new CustomAttributeBuilder (ci, new[] { headerObj });
            return attrBuilder;
        }

        void RemoveFakeFile()
        {
            if (File.Exists (GetFullPath(_fakeXlsxFileName))) {
                File.Delete (_fakeXlsxFileName);
            }
        }

        private string CreateFakeZeroLengthXlsxFile()
        {
            using (var stream = new FileStream (GetFullPath(_fakeXlsxFileName),FileMode.Create, FileAccess.Write)) {
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

            using (var stream = new FileStream (GetFullPath (_fakeXlsxFileName), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        private string CreateTestFileWithSingleStringCell()
        {
            IWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet();

            sheet.CreateRow (10).CreateCell (10).SetCellValue (_mainCellText);

            using (var stream = new FileStream (GetFullPath (_fakeXlsxFileName), FileMode.Create, FileAccess.Write)) {
                    
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

            using (var stream = new FileStream (GetFullPath (_fakeXlsxFileName), FileMode.Create, FileAccess.Write)) {
                    
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

            using (var stream = new FileStream (GetFullPath (_fakeXlsxFileName), FileMode.Create, FileAccess.Write)) {
                    
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

            using (var stream = new FileStream (GetFullPath (_fakeXlsxFileName), FileMode.Create, FileAccess.Write)) {
                    
                book.Write (stream);
                return stream.Name;
            }
        }

        [Headless]
        class FakeHeadlessModelClass
        {
            public string MainCell { get; set; }
            public string ReadOnlyProperty { get; } = "Read Only Property";
        }

        class FakeModelClassWithTestHeaderNamedProperty
        {
            public string TestHeader { get; set; }
            public string ReadOnlyProperty { get; } = "Read Only Property";
        }

        class FakeModelClassWithWrongHeaderAttribute
        {
            [Header("Wrong Header")]
            public string TestHeader { get; set; }
            public string ReadOnlyProperty { get; } = "Read Only Property";
        }

        class FakeModelClassWithoutParameterlessConstructor
        {
            public FakeModelClassWithoutParameterlessConstructor (string param)
            {
                TestHeader = param;
            }

            public string TestHeader { get; set; }
            public string ReadOnlyProperty { get; } = "Read Only Property";
        }

        #endregion
    }
}
