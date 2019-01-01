using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NUnit.Framework;

namespace NpoiExcel.Tests.IntegrationalTests
{
    [TestFixture]
    public class CellValueIntegrationalTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-us");
        }


        #region IntImplicit

        [Test]
        public void IntImplicit_ICellIsNull_ReturnsDefaultValue()
        {
            // Arrange:
            var cellValue = new CellValue(null);

            // Action:
            int intValue = cellValue;

            // Assert:
            Assert.That (default(int) == intValue);
        }

        [TestCase (0.0, 0)]
        [TestCase (-9876543.0123456789, -9876543)]
        [TestCase (9876543.0123456789, 9876543)]
        public void IntImplicit_ICellIsNumeric_ReturnsValue(double value, int expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetNumericCell(value));

            // Action:
            int intValue = cellValue;

            // Assert:
            Assert.That (expected == intValue);
        }
        
        [TestCase ("0.0", 0)]
        [TestCase ("0", 0)]
        [TestCase (" -9876543.0123456789", -9876543)]
        [TestCase ("9876543.0123456789 ", 9876543)]
        [TestCase (" 9876543", 9876543)]
        [TestCase ("-9876543 ", -9876543)]
        [TestCase ("Да", 1)]
        [TestCase ("да", 1)]
        [TestCase ("дА", 1)]
        [TestCase ("ДА", 1)]
        [TestCase ("Yes", 1)]
        public void IntImplicit_CellIsNumericOrTrueString_ReturnsValue(string value, int expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            int actual = cellValue;

            // Assert:
            Assert.That (expected == actual);
        }

        [TestCase ("d0.0")]
        [TestCase ("r0")]
        [TestCase ("-9876543.012+3456789")]
        [TestCase ("9876543.01 23456789")]
        [TestCase ("d9876543")]
        [TestCase ("-987/6543")]
        [TestCase ("No")]
        [TestCase ("nO")]
        [TestCase ("Нет")]
        public void IntImplicit_CellIsString_ReturnsDefaultValue(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            int actual = cellValue;

            // Assert:
            Assert.That (default(int) == actual);
        }

        [TestCase (true)]
        public void IntImplicit_CellIsTrue_ReturnsOne(bool value)
        {
            // Arrange:
            var cellValue = new CellValue(GetBooleanCell(value));

            // Action:
            int actual = cellValue;

            // Assert:
            Assert.That (1 == actual);
        }

        [TestCase (false)]
        public void IntImplicit_CellIsFalse_ReturnsZero(bool value)
        {
            // Arrange:
            var cellValue = new CellValue(GetBooleanCell(value));

            // Action:
            int actual = cellValue;

            // Assert:
            Assert.That (0 == actual);
        }

        [TestCase ("11.12.2015", 42349)]
        [TestCase ("01.01.1900", 1)]
        [TestCase ("25.07.2541 0:34:12", 234328)]
        [TestCase ("13.03.1346", -1)]
        public void IntImplicit_CellIsDateTime_ReturnsExpectedValue(string value, int expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetDefaultCell(DateTime.Parse (value)));

            // Action:
            int actual = cellValue;

            // Assert:
            Assert.That (expected == actual);
        }

        #endregion


        #region DoubleImplicit

        [Test]
        public void DoubleImplicit_ICellIsNull_ReturnsDefaultValue()
        {
            // Arrange:
            var cellValue = new CellValue(null);

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (default(double)));
        }
        
        [TestCase (0.0, 0.0)]
        [TestCase (-9876543.0123456789, -9876543.0123456789)]
        [TestCase (9876543.0123456789, 9876543.0123456789)]
        public void DoubleImplicit_ICellIsNumeric_ReturnsValue(double value, double expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetNumericCell(value));

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (expected));
        }
        
        [TestCase ("0.0", 0.0)]
        [TestCase ("0", 0.0)]
        [TestCase (" -9876543.0123456789", -9876543.0123456789)]
        [TestCase ("9876543.0123456789 ", 9876543.0123456789)]
        [TestCase (" 9876543", 9876543.0)]
        [TestCase ("-9876543 ", -9876543.0)]
        [TestCase ("Да", 1.0)]
        [TestCase ("да", 1.0)]
        [TestCase ("дА", 1.0)]
        [TestCase ("ДА", 1.0)]
        [TestCase ("Yes", 1.0)]
        public void DoubleImplicit_CellIsNumericOrTrueString_ReturnsValue(string value, double expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (expected));
        }

        [TestCase ("d0.0")]
        [TestCase ("r0")]
        [TestCase ("-9876543.012+3456789")]
        [TestCase ("9876543.01 23456789")]
        [TestCase ("d9876543")]
        [TestCase ("-987/6543")]
        [TestCase ("No")]
        [TestCase ("nO")]
        [TestCase ("Нет")]
        public void DoubleImplicit_CellIsString_ReturnsDefaultValue(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (default(double)));
        }

        [TestCase (true)]
        public void DoubleImplicit_CellIsTrue_ReturnsOne(bool value)
        {
            // Arrange:
            var cellValue = new CellValue(GetBooleanCell(value));

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (1.0));
        }

        [TestCase (false)]
        public void DoubleImplicit_CellIsFalse_ReturnsZero(bool value)
        {
            // Arrange:
            var cellValue = new CellValue(GetBooleanCell(value));

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (0.0));
        }

        [TestCase ("11.12.2015", 42349.0)]
        [TestCase ("01.01.1900", 1.0)]
        [TestCase ("25.07.2541 0:34:12", 234328.02375)]
        [TestCase ("13.03.1346", -1.0)]
        public void DoubleImplicit_CellIsDateTime_ReturnsExpectedValue(string value, double expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetDefaultCell(DateTime.Parse (value)));

            // Action:
            double actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (expected));
        }

        #endregion


        #region BoolImplicit

        [Test]
        public void BoolImplicit_ICellIsNull_ReturnsDefaultValue()
        {
            // Arrange:
            var cellValue = new CellValue(null);

            // Action:
            bool actual = cellValue;

            // Assert:
            Assert.That (default(bool) == actual);
        }
        
        [TestCase (0.0, false)]
        [TestCase (-9876543.0123456789, true)]
        [TestCase (9876543.0123456789, true)]
        public void BoolImplicit_ICellIsNumeric_ReturnsExpectedValue(double value, bool expected)
        {
            // Arrange:
            var cellValue = new CellValue(GetNumericCell(value));

            // Action:
            bool actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (expected));
        }

        [TestCase( "0.0", false )]
        [TestCase( "1.0", true )]
        [TestCase( "0", false )]
        [TestCase( "1", true )]
        [TestCase(" -9876543.0123456789", true)]
        [TestCase("9876543.0123456789 ", true)]
        [TestCase( " 9876543", true )]
        [TestCase( "-9876543 ", true )]
        [TestCase( "Да", true )]
        [TestCase( "да", true )]
        [TestCase( "дА", true )]
        [TestCase( "ДА", true )]
        [TestCase( "Yes", true )]
        public void BoolImplicit_CellIsSomeOrTrueString_ReturnsExpectedValue(string value, bool expected)
        {
            // Arrange:
            var cellValue = new CellValue( GetStringCell(value) );

            // Action:
            bool actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (expected));
        }

        [TestCase ("No")]
        [TestCase (" nO ")]
        [TestCase (" NO")]
        [TestCase ("no ")]
        [TestCase ("Нет")]
        [TestCase (" нЕт")]
        [TestCase ("нЕТ")]
        [TestCase ("нет   ")]
        [TestCase ("    НеТ  ")]
        public void BoolImplicit_CellIsFalseString_ReturnsFalse(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            bool actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (false));
        }

        [TestCase (true)]
        [TestCase (false)]
        public void BoolImplicit_CellIsBoolean_ReturnsTrue(bool value)
        {
            // Arrange:
            var cellValue = new CellValue(GetBooleanCell(value));

            // Action:
            bool actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (value));
        }

        [TestCase ("11.12.2015")]
        [TestCase ("01.01.1900")]
        [TestCase ("25.07.2541 0:34:12")]
        [TestCase ("13.03.1346")]
        public void BoolImplicit_CellIsDateTime_ReturnsFalse(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetDefaultCell(DateTime.Parse (value)));

            // Action:
            bool actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (true));
        }

        #endregion


        #region StringImplicit

        [Test]
        public void StringImplicit_ICellIsNull_ReturnsDefaultValue()
        {
            // Arrange:
            var cellValue = new CellValue(null);

            // Action:
            string actual = cellValue;

            // Assert:
            Assert.That (default(string) == actual);
        }
        
        [TestCase ("    1ffg_ 9340 $^$_ 334[e[e[re[=++!~ Ёёдпвза да.бю,  ")]
        public void StringImplicit_CellIsString_ReturnsExpectedValue(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            string actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (value));
        }

        [TestCase ("11.12.2015", "42349")]
        [TestCase ("01.01.1900", "1")]
        [TestCase ("25.07.2541 0:34:12", "234328.02375")]
        [TestCase ("13.03.1346", "-1")]
        public void StringImplicit_CellIsDateTime_ReturnsStringEquialentOfDateTime(string value, string expected)
        {
            // Arrange:
            var cellValue = new CellValue( GetDefaultCell( DateTime.Parse( value )));

            // Action:
            string actual = cellValue;

            // Assert:
            Assert.That ( actual, Is.EqualTo( expected ) );
        }

        #endregion


        #region DateTimeImplicit

        [Test]
        public void DateTimeImplicit_ICellIsNull_ReturnsDefaultValue()
        {
            // Arrange:
            var cellValue = new CellValue(null);

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (default(DateTime) == actual);
        }

        [TestCase ((double)int.MaxValue)]
        [TestCase ((double)int.MinValue)]
        [TestCase (-9876543.0123456789)]
        [TestCase (9876543.0123456789)]
        [TestCase (-693595.0)]
        [TestCase (0.0)]
        public void DateTimeImplicit_ICellIs16BitNumericOrNegative693595_ReturnsDefaultValue(double value)
        {
            // Arrange:
            var cellValue = new CellValue(GetNumericCell(value));

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (default(DateTime)));
        }

        [TestCase ((double)ushort.MaxValue)]
        [TestCase ((double)-ushort.MaxValue)]
        [TestCase (-1.0123456789)]
        [TestCase (1.0123456789)]
        public void DateTimeImplicit_ICellIs16BitNumeric_ReturnsNotDefaultValue(double value)
        {
            // Arrange:
            var cellValue = new CellValue(GetNumericCell(value));

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (actual, Is.Not.EqualTo (default(DateTime)));
        }



        [TestCase (true)]
        [TestCase (false)]
        public void DateTimeImplicit_CellIsBoolean_ReturnsDefaultValue(bool value)
        {
            // Arrange:
            var cellValue = new CellValue(GetBooleanCell(value));

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (default(DateTime)));
        }

        [TestCase ("11.12.2015")]
        [TestCase (" 01.01.1900")]
        [TestCase ("25.07.2541 0:34:12 ")]
        [TestCase ("  13.03.1346 ")]
        public void DateTimeImplicit_CellIsStringWithDateTime_ReturnsNotDefaultValue(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (actual, Is.Not.EqualTo (default(DateTime)));
        }

        [TestCase ("11.12.2015-11.12.2015")]
        [TestCase ("01.01.1990+01.01.1900")]
        [TestCase ("a25.07.2541 0:34:12")]
        [TestCase ("13.03.1346 _")]
        public void DateTimeImplicit_CellIsStringWithNoDateTime_ReturnsDefaultValue(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetStringCell(value));

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (actual, Is.EqualTo (default(DateTime)));
        }

        //[TestCase ("11.12.2015")]
        //[TestCase ("01.01.1900")]
        [TestCase ("25.07.2541 0:34:12")]
        //[TestCase ("13.03.1346")]
        public void DateTimeImplicit_CellIsDateTimeNotDefault_ReturnsNotDefaultValue(string value)
        {
            // Arrange:
            var cellValue = new CellValue(GetDefaultCell(DateTime.Parse (value)));

            // Action:
            DateTime actual = cellValue;

            // Assert:
            Assert.That (actual, Is.Not.EqualTo (default(DateTime)));
        }

        #endregion



        #region Factory

        private ICell GetNumericCell ( double value )
        {
            var book = new XSSFWorkbook();
            var cell = book.CreateSheet ("0").CreateRow (0).CreateCell (0);
            cell.SetCellType (CellType.Numeric);
            cell.SetCellValue (value);
            return cell;
        }

        private ICell GetStringCell ( string value )
        {
            var book = new XSSFWorkbook();
            var cell = book.CreateSheet ("0").CreateRow (0).CreateCell (0);
            cell.SetCellType (CellType.String);
            cell.SetCellValue (value);
            return cell;
        }

        private ICell GetBooleanCell ( bool value )
        {
            var book = new XSSFWorkbook();
            var cell = book.CreateSheet ("0").CreateRow (0).CreateCell (0);
            cell.SetCellType (CellType.Boolean);
            cell.SetCellValue (value);
            return cell;
        }

        private ICell GetDefaultCell ( DateTime value )
        {
            var book = new XSSFWorkbook();
            var cell = book.CreateSheet( "0" ).CreateRow( 0 ).CreateCell( 0 );
            cell.SetCellValue( value );
            return cell;
        }

        #endregion
    }
}
