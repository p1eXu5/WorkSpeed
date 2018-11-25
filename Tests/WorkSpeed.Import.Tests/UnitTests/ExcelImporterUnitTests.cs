using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;
using WorkSpeed.Import.Attributes;
using static WorkSpeed.Import.Tests.TestHelper;

namespace WorkSpeed.Import.Tests.UnitTests
{
    [TestFixture]
    public class ExcelImporterUnitTests
    {
        #region GetFirstCell

        [Test]
        public void ImportDataFromExcel_XlsxFileDoesNotContainData_ReturnsEmptyCollection()
        {
            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(GetFullPath ("empty.xlsx"), typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel_ZeroLenghtFileWithCorrectExtension_ReturnsEmptyCollection()
        {
            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(CreateFakeZeroLengthXlsxFile (), typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [Test]
        public void ImportDataFromExcel_OneColumnOneProperty_ReturnsTheMostTopLeftCell ()
        {
            // Arrange:
            var fileName = CreateTestFile ();

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeModelClass));

            // Assert:
            Assert.That (_mainCellText == resColl.OfType<FakeModelClass>().First().MainCell);
        }

        [Test]
        public void ImportDataFromExcel_MultiColumnOneProperty_ReturnsEmptyCollection ()
        {
            // Arrange:
            var fileName = CreateTestFile (new TableRect(5, 5, 5), (float)1.0, 5, 5);

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel(fileName, typeof(FakeModelClass));

            // Assert:
            Assert.That (0 == resColl.Count);
        }

        [TestCase ("", "string")]
        [TestCase (" ", "string")]
        [TestCase ("value", "string")]
        [TestCase ("1", "string")]
        [TestCase ("1.02", "string")]
        [TestCase (",#2-", "string")]
        [TestCase ("Да", "string")]
        public void ImportDataFromExcel_StringProperty_CanReadStringCell(string cellValue, string propertyType)
        {
            // Arrange:
            Type modelType = GetModelType(propertyType);

            // Action:
            var resColl = ExcelImporter.ImportDataFromExcel (CreateTestHeadedFile (cellValue), modelType);

            // Assert:
            var element = resColl.GetEnumerator().Current;
            Assert.That (cellValue == element?.GetType().GetProperties()[0].GetValue (element).ToString());
        }

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
        public void ImportDataFromExcel_IntProperty_CanReadIntAndBoolCell()
        {

        }

        [TestCase ("1a", "int")]
        [TestCase ("d1.02", "int")]
        [TestCase ("_-1", "int")]
        [TestCase ("-1-", "int")]
        [TestCase ("-1.,00123", "int")]
        [TestCase ("Даc", "int")]
        [TestCase ("Нетc", "int")]
        [TestCase ("Yesc", "int")]
        [TestCase ("Noc", "int")]
        public void ImportDataFromExcel_IntProperty_CanNotReadStringCell()
        {

        }

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
        public void ImportDataFromExcel_DoubleProperty_CanReadDoubleAndBoolCell()
        {

        }

        [TestCase ("1a", "double")]
        [TestCase ("d1.02", "double")]
        [TestCase ("_-1", "double")]
        [TestCase ("-1-", "double")]
        [TestCase ("-1.,00123", "double")]
        [TestCase ("Даc", "double")]
        [TestCase ("Нетc", "double")]
        [TestCase ("Yesc", "double")]
        [TestCase ("Noc", "double")]
        public void ImportDataFromExcel_DoubleProperty_CanNotReadStringCell()
        {

        }

        [TestCase ("", "bool", false)]
        [TestCase (" ", "bool", false)]
        [TestCase ("Да", "bool", true)]
        [TestCase ("дА", "bool", true)]
        [TestCase ("Yes", "bool", true)]
        [TestCase ("yEs", "bool", true)]
        [TestCase ("Нет", "bool", false)]
        [TestCase ("НеТ", "bool", false)]
        [TestCase ("NO", "bool", false)]
        public void ImportDataFromExcel_BoolProperty_CanReadBoolCell()
        {

        }

        [TestCase ("1", "bool")]
        [TestCase ("-1", "bool")]
        [TestCase ("1a", "bool")]
        [TestCase ("d1.02", "bool")]
        [TestCase ("_-1", "bool")]
        [TestCase ("-1-", "bool")]
        [TestCase ("-1.,00123", "bool")]
        [TestCase ("0.00123", "bool")]
        [TestCase ("1,00123", "bool")]
        [TestCase ("-1.00123", "bool")]
        [TestCase ("-0,00123", "bool")]
        [TestCase ("Даc", "bool")]
        [TestCase ("Нетc", "bool")]
        [TestCase ("Yesc", "bool")]
        [TestCase ("Noc", "bool")]
        public void ImportDataFromExcel_BoolProperty_CanNotReadStringIntAndDoubleCell()
        {

        }

        [TearDown]
        public void Cleanup()
        {
            RemoveFakeFile();
        }

        // TODO: Property Count Assertions

        #endregion

        #region Factory

        private const string _fakeFileName = "fakefile.xlsx";
        private const string _mainCellText = "Main Cell";
        private const string _testHeaderName = "TestHeader";

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

                            // field:
                        FieldBuilder fieldBuilder = typeBuilder.DefineField ($"_testProperty{i}", typeof(string), FieldAttributes.Private);
                        PropertyBuilder propertyBuilder = typeBuilder.DefineProperty ($"TestProperty{i}", PropertyAttributes.HasDefault, typeof(string), null);

                        MethodBuilder getterBuilder = typeBuilder.DefineMethod ($"get_TestProperty{i}", getSetAttr, typeof(string), Type.EmptyTypes);
                        LoadIlToGetter (getterBuilder, fieldBuilder);

                        MethodBuilder setterBuilder = typeBuilder.DefineMethod ($"set_TestProperty{i}", getSetAttr, null, new Type[] {typeof(string)});
                        LoadIlToSetter (setterBuilder, fieldBuilder);

                        propertyBuilder.SetGetMethod (getterBuilder);
                        propertyBuilder.SetSetMethod (setterBuilder);

                        var attrBuilder = GetHeaderAttributeBuilder(_testHeaderName);
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
            ConstructorInfo ci = typeof(HeaderAttribute).GetConstructor (new Type[]{ typeof(string) });
            CustomAttributeBuilder attrBuilder = new CustomAttributeBuilder (ci, new object[] { headerObj });
            return attrBuilder;
        }

        void RemoveFakeFile()
        {
            if (File.Exists (GetFullPath(_fakeFileName))) {
                File.Delete (_fakeFileName);
            }
        }

        private string CreateFakeZeroLengthXlsxFile()
        {
            using (var stream = new FileStream (GetFullPath(_fakeFileName),FileMode.Create, FileAccess.Write)) {
                return stream.Name;
            }
        }

        private string CreateTestFile (TableRect columns, float density, byte mainTop, byte mainLeft)
        {
            Random rnd = new Random((int)DateTime.Now.TimeOfDay.TotalSeconds);
            try {
                IWorkbook book = new XSSFWorkbook();
                ISheet sheet = book.CreateSheet();

                var Nc = columns.Right - columns.Left;
                var nc = density > 1 
                            ? (density - Math.Floor (density)) * Nc 
                            : density * Nc;

                for (int j = columns.Top; j <= columns.Bottom; j++) {

                    var row = sheet.CreateRow (j);

                    for (int i = 0; i < nc; i++) {

                        int nextCell = columns.Left + rnd.Next (Nc);

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

                using (var stream = new FileStream (GetFullPath (_fakeFileName), FileMode.Create, FileAccess.Write)) {
                    
                    book.Write (stream);
                    return stream.Name;
                }
            }
            catch {
                throw;
            }
        }

        private string CreateTestFile()
        {
            try {
                IWorkbook book = new XSSFWorkbook();
                ISheet sheet = book.CreateSheet();

                sheet.CreateRow (0).CreateCell (0).SetCellValue ("");
                sheet.CreateRow (10).CreateCell (10).SetCellValue (_mainCellText);

                using (var stream = new FileStream (GetFullPath (_fakeFileName), FileMode.Create, FileAccess.Write)) {
                    
                    book.Write (stream);
                    return stream.Name;
                }
            }
            catch {
                throw;
            }
        }

        private string CreateTestHeadedFile (string testValue)
        {
            try {
                IWorkbook book = new XSSFWorkbook();
                ISheet sheet = book.CreateSheet();

                sheet.CreateRow (9).CreateCell (10).SetCellValue (_testHeaderName);
                sheet.CreateRow (10).CreateCell (10).SetCellValue (testValue);

                using (var stream = new FileStream (GetFullPath (_fakeFileName), FileMode.Create, FileAccess.Write)) {
                    
                    book.Write (stream);
                    return stream.Name;
                }
            }
            catch {
                throw;
            }
        }

        [Headless]
        class FakeModelClass
        {
            public string MainCell { get; set; }
        }

        #endregion
    }
}
