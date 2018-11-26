using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WorkSpeed.Import.Attributes;

namespace WorkSpeed.Import
{
    public class ExcelImporter : IConcreteImporter
    {
        private static ExcelImporter _excelImporter;

        private ExcelImporter() {}

        #region Properties

        public static ExcelImporter ExcelImporterInstance => _excelImporter ?? (_excelImporter = new ExcelImporter());

        public string FileExtension { get; } = ".xlsx";
        public Func<string, Type, ICollection> ImportData { get; } = ImportDataFromExcel;

        #endregion

        #region Methods

        public static ICollection ImportDataFromExcel(string fileName, Type type)
        {
            if (type.GetConstructors().Count (c => c.IsPublic && c.GetParameters().Length == 0) == 0) {
                return GetEmptyCollection(type);
            }

            using (Stream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                try {
                    IWorkbook book = new XSSFWorkbook (stream);
                    ISheet sheet = book.GetSheetAt (0);

                    var tableRect = GetFirstCell (sheet);

                    if (null == tableRect) {
                        return GetEmptyCollection (type);
                    }

                    return TryFillCollection (sheet, (TableRect)tableRect, type) ?? GetEmptyCollection (type);
                }
                catch (ZipException) {

                    return GetEmptyCollection (type);
                }
            }
        }

        /// <summary>
        /// Returns data area in Excel table.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static TableRect? GetFirstCell(ISheet sheet)
        {
            TableRect tableRect = GetRect();

            if (tableRect.Equals (new TableRect (-1))) {
                return null;
            }

            var j = tableRect.Top;
            var i = tableRect.Left;

            // If file contains only one cell:
            if (tableRect.Top == tableRect.Bottom && tableRect.Left == tableRect.Right - 1) {

                if (sheet.GetRow (tableRect.Top).GetCell (tableRect.Left).CellType == CellType.Blank) {
                    return null;
                }

                return tableRect;
            }

            // If file contains only one column:
            if (tableRect.Left == tableRect.Right - 1) {

                while (sheet.GetRow (j)?.GetCell (i) == null || sheet.GetRow (j).GetCell (i).CellType == CellType.Blank) {
                    if (j == tableRect.Bottom) {
                        break;
                    }
                    ++j;
                }


                if (j == tableRect.Bottom && sheet.GetRow (j).GetCell (i).CellType == CellType.Blank) {
                    return null;
                }

                tableRect.Top = j;
                return tableRect;
            }
            
            // If file contains one or more rows and some columns:
            do {
                IRow row = sheet.GetRow (j);

                if (row != null) {
                    if (tableRect.Left > 0) {
                        if (tableRect.Left > row.FirstCellNum) {
                            tableRect.Left = row.FirstCellNum;
                        }
                    }

                    if (tableRect.Right < row.LastCellNum) {
                        tableRect.Right = row.LastCellNum;
                    }
                }

                ++j;
            } while (j <= tableRect.Bottom);

            return tableRect;
            

            #region Functions

            TableRect GetRect()
            {
                TableRect rect;

                rect.Top = sheet.FirstRowNum;
                rect.Bottom = sheet.LastRowNum;

                if (rect.Bottom < rect.Top) throw new ArgumentException();

                rect.Left = sheet.GetRow (rect.Top)?.FirstCellNum ?? -1;
                if (rect.Left == -1) {
                    return new TableRect(-1);
                }

                rect.Right = sheet.GetRow (rect.Bottom).LastCellNum;

                return rect;
            }

            #endregion
        }

        [SuppressMessage ("ReSharper", "PossibleNullReferenceException")]
        private static ICollection TryFillCollection (ISheet sheet, TableRect rect, Type type)
        {
            if (type.GetProperties ().Count(p => p.CanWrite) != rect.Right - rect.Left) {
                return GetEmptyCollection (type);
            }

            if (   !type.GetCustomAttributes (false).Any() 
                || (type.GetCustomAttributes (false)[0] is HeadlessAttribute attr && !attr.IsHeadless)) {

                if (!CheckHeaders()) {
                    return GetEmptyCollection (type);
                }

                ++rect.Top;
            }

            return FillModelCollection();


            #region Functions

            ArrayList FillModelCollection()
            {
                ArrayList list = new ArrayList(rect.Bottom - rect.Top + 1);

                for (var j = rect.Top; j <= rect.Bottom; ++j) {

                    object obj = Activator.CreateInstance (type);
                    var i = rect.Left;

                    var row = sheet.GetRow (j);
                    if (null == row) continue;

                    foreach (var propertyInfo in type.GetProperties()) {

                        if (propertyInfo.SetMethod == null) continue;

                        bool isSet = false;
                        ICell cell = row.GetCell (i);

                        if (propertyInfo.PropertyType.FullName == typeof(string).FullName) {

                            string stringValue = default(string);
                            SetStringValue(cell, ref stringValue);

                            obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, stringValue);
                            isSet = true;
                        }
                        else if (propertyInfo.PropertyType.FullName == typeof(int).FullName) {

                            int intValue = default(int);
                            SetIntValue(cell, ref intValue);

                            obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, intValue);
                            isSet = true;
                        }
                        else if (propertyInfo.PropertyType.FullName == typeof(double).FullName) {

                            double doubleValue = default(double);
                            SetDoubleValue(cell, ref doubleValue);

                            obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, doubleValue);
                            isSet = true;
                        }
                        else if (propertyInfo.PropertyType.FullName == typeof(bool).FullName) {

                            bool boolValue = default(bool);
                            SetBoolValue(cell, ref boolValue);

                            obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, boolValue);
                            isSet = true;
                        }

                        if (!isSet) {
                            return null;
                        }
                    }

                    list.Add (obj);
                    ++i;
                }

                return list;
            }

            bool CheckHeaders ()
            {
                var i = rect.Left;

                foreach (var propertyInfo in type.GetProperties().Where (p => p.CanWrite)) {

                    var propertyAttr = propertyInfo.GetCustomAttributes (false)
                                                   .FirstOrDefault (a => a is HeaderAttribute) as HeaderAttribute;

                    var cellHeader = sheet.GetRow (rect.Top).GetCell (i).StringCellValue;

                    if (null == propertyAttr) {
                        if (!propertyInfo.Name.Equals (Regex.Replace (cellHeader, @"\s+", String.Empty),
                                                       StringComparison.InvariantCulture)) {
                            return false;
                        }
                    }
                    else if (propertyAttr.Header != cellHeader) {
                        return false;
                    }

                    ++i;
                }

                return true;
            }

            #endregion
        }

        private static void SetStringValue (ICell cell, ref string stringValue)
        {
            if (cell == null) return;

            if (CellType.Numeric == cell.CellType) {
                try {
                    stringValue = cell.NumericCellValue.ToString (CultureInfo.InvariantCulture);
                }
                catch (OverflowException) {
                    stringValue = "0.0";
                }
            }
            else if (CellType.Boolean == cell.CellType) {
                stringValue = cell.BooleanCellValue ? "Да" : "Нет";
            }
            else if (CellType.String == cell.CellType) {
                stringValue = cell.StringCellValue;
            }
        }

        private static void SetIntValue (ICell cell, ref int intValue)
        {
            if (cell == null) return;

            if (CellType.Numeric == cell.CellType) {
                try {
                    intValue = Convert.ToInt32 (cell.NumericCellValue);
                }
                catch (OverflowException) {
                    intValue = 0;
                }
            }
            else if (CellType.Boolean == cell.CellType) {
                if (cell.BooleanCellValue) {
                    intValue = 1;
                }
            }
            else if (CellType.String == cell.CellType) {
                var cellValue = cell.StringCellValue;

                if (!String.IsNullOrWhiteSpace (cellValue)) {
                    if (cellValue.ToUpperInvariant() == "Да".ToUpperInvariant()
                        || cellValue.ToUpperInvariant() == "Yes".ToUpperInvariant()) {
                        intValue = 1;
                    }
                    else {
                        cellValue = cellValue.Replace (',', '.');

                        try {
                            intValue = Convert.ToInt32 (Double.Parse (cellValue));
                        }
                        catch {
                        }
                    }
                }
            }
        }

        private static void SetDoubleValue (ICell cell, ref double doubleValue)
        {
            if (cell == null) return;

            if (CellType.Numeric == cell.CellType) {
                try {
                    doubleValue = cell.NumericCellValue;
                }
                catch (OverflowException) {
                    doubleValue = 0.0;
                }
            }
            else if (CellType.Boolean == cell.CellType) {

                if (cell.BooleanCellValue) {
                    doubleValue = 1.0;
                }
            }
            else if (CellType.String == cell.CellType) {

                var cellValue = cell.StringCellValue;

                if (!String.IsNullOrWhiteSpace (cellValue)) {

                    if (cellValue.ToUpperInvariant() == "Да".ToUpperInvariant()
                        || cellValue.ToUpperInvariant() == "Yes".ToUpperInvariant()) {

                        doubleValue = 1.0;
                    }
                    else {
                        cellValue = cellValue.Replace (',', '.');

                        try {
                            doubleValue = Double.Parse (cellValue);
                        }
                        catch {
                        }
                    }
                }
            }
        }

        private static void SetBoolValue (ICell cell, ref bool boolValue)
        {
            if (cell == null) return;

            if (CellType.Numeric == cell.CellType) {

                try {
                    boolValue = !cell.NumericCellValue.Equals (0.0);
                }
                catch (OverflowException) {
                    boolValue = false;
                }
            }
            else if (CellType.Boolean == cell.CellType) {

                boolValue = cell.BooleanCellValue;
            }
            else if (CellType.String == cell.CellType) {

                var cellValue = cell.StringCellValue;

                if (!String.IsNullOrWhiteSpace (cellValue)) {

                    if (cellValue.ToUpperInvariant() == "Да".ToUpperInvariant()
                        || cellValue.ToUpperInvariant() == "Yes".ToUpperInvariant()) {

                        boolValue = true;
                    }
                }
            }
        }

        private static ICollection GetEmptyCollection(Type type)

        {
            Type t = typeof(List<>);
            var constr = t.MakeGenericType (type);
            return (ICollection)Activator.CreateInstance (constr);
        }
        #endregion
    }
}
