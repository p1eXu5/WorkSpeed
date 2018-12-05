using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using ExcelImporter.Attributes;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WorkSpeed.Import;

namespace ExcelImporter
{
    [SuppressMessage("ReSharper", "EmptyGeneralCatchClause")]
    public static class ExcelImporter
    {
        private const string XLS_FILE = ".xls";
        private const string XLSX_FILE = ".xlsx";

        static readonly Dictionary<string, int> _propertyToCellColumn = new Dictionary<string, int>();

        #region Properties

        public static HashSet<string> FileExtensions { get; } = new HashSet<string> { XLS_FILE, XLSX_FILE };

        #endregion

        #region Methods

        public static ICollection ImportDataFromExcel(string fileName, Type type, int sheetIndex)
        {
            var fileExtension = Path.GetExtension(fileName);
            if (!FileExtensions.Contains(fileExtension)) throw new FileFormatException($"{fileName} has wrong file extansion!");

            if (type.GetConstructors().Count(c => c.IsPublic && c.GetParameters().Length == 0) == 0) {
                throw new TypeAccessException($"{type} has not public parameterized constructor!");
            }

            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {

                // Load sheetTable:
                ISheet sheet;

                try {
                    sheet = GetSheet (stream, sheetIndex, fileExtension);
                }
                catch {
                    throw new FileFormatException($"{fileName} has invalid file format or has no sheetTable with {sheetIndex} index!");
                }

                // Load SheetTable:
                SheetTable sheetTable;

                try {
                    sheetTable = new SheetTable (sheet);
                }
                catch (ArgumentException) {
                    return GetEmptyCollection (type);
                }

                // Load headers map:
                var headersMap = GetHeaderMap (type, sheetTable);
                if (!headersMap.Keys.Any()) return GetEmptyCollection (type);

                return FillModelCollection(sheetTable, type);
            }
        }

        /// <summary>
        /// Returns public writable properties with no HiddenAttribute
        /// </summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <returns><see cref="Array"/></returns>
        public static HashSet<(string[] headers, string name)> GetPropertyNames (Type type)
        {
            return  new HashSet<(string[] headers, string name)> 
                        (
                                type.GetProperties()
                                    .Where (p => p.CanWrite && !p.GetCustomAttributes(typeof(HiddenAttribute)).Any())
                                    .Select (p => (new [] {""}, p.Name))
                        );
        }

        public static IEnumerable<PropertyInfo> GetPropertyInfos (this Type type)
        {
            return type.GetProperties()
                       .Where (p => p.CanWrite && !p.GetCustomAttributes (typeof(HiddenAttribute)).Any());
        }

        public static HashSet<(string[] headers, string name)> GetPropertyHeaders (Type type)
        {
            return  new HashSet<(string[] headers, string propertyName)> 
                        (
                                type.GetProperties()
                                    .Where(p => p.CanWrite && !p.GetCustomAttributes(typeof(HiddenAttribute)).Any())
                                    .Select(p =>
                                            {
                                                var attr = p.GetCustomAttributes (typeof (HeaderAttribute)).Select (a => ((HeaderAttribute)a).Header).ToArray();
                                                return (attr, p.Name);
                                            })
                       );
        }

        public static bool IsHeaderless (this Type type)
        {
            return type.GetCustomAttributes (typeof (HeaderlessAttribute)).Any();
        }

        public static ISheet GetSheet (Stream stream, int sheetIndex, string excelVerion = XLSX_FILE)
        {
            IWorkbook book;

            if (XLS_FILE == excelVerion) {
                book = new HSSFWorkbook(stream);
            }
            else {
                book = new XSSFWorkbook(stream);
            }

            return book.GetSheetAt(sheetIndex);
        }

        public static Dictionary<string, int> GetHeaderMap (Type type, SheetTable sheetTable)
        {
            var map = new Dictionary<string, int>();
            var propertyTuples = type.IsHeaderless() ? GetPropertyNames (type) : GetPropertyHeaders (type);

            if (!propertyTuples.Any()) return map;

            for (var i = sheetTable.StartCell.Column; i < sheetTable.EndCell.Column; ++i) {

                foreach (var tuple in propertyTuples) {

                    if (tuple.headers.Contains (sheetTable[i])) {

                        map[tuple.name] = i;
                        propertyTuples.Remove (tuple);

                        break;
                    }
                }

                if (!propertyTuples.Any()) return map;
            }

            if (propertyTuples.Any()) {
                map.Clear();
            }

            return map;
        }

        /// <summary>
        /// Returns data area in Excel table.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static TableRect? GetFirstCell(ISheet sheet)
        {
            TableRect tableRect = GetRect(sheet);

            if (tableRect.Equals(new TableRect(-1))) {
                return null;
            }

            var j = tableRect.Top;
            var i = tableRect.Left;

            // If file contains only one cell:
            if (tableRect.Top == tableRect.Bottom && tableRect.Left == tableRect.Right - 1) {

                if (sheet.GetRow(tableRect.Top).GetCell(tableRect.Left).CellType == CellType.Blank) {
                    return null;
                }

                return tableRect;
            }

            // If file contains only one column:
            if (tableRect.Left == tableRect.Right - 1) {

                while (sheet.GetRow(j)?.GetCell(i) == null || sheet.GetRow(j).GetCell(i).CellType == CellType.Blank) {
                    if (j == tableRect.Bottom) {
                        break;
                    }
                    ++j;
                }


                if (j == tableRect.Bottom && sheet.GetRow(j).GetCell(i).CellType == CellType.Blank) {
                    return null;
                }

                tableRect.Top = j;
                return tableRect;
            }

            // If file contains one or more rows and some columns:
            do {
                IRow row = sheet.GetRow(j);

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


            #endregion
        }

        private static TableRect GetRect(ISheet sheet)
        {
            TableRect rect;

            rect.Top = sheet.FirstRowNum;
            rect.Bottom = sheet.LastRowNum;

            if (rect.Bottom < rect.Top) throw new ArgumentException();

            rect.Left = sheet.GetRow(rect.Top)?.FirstCellNum ?? -1;
            if (rect.Left == -1) {
                return new TableRect(-1);
            }

            rect.Right = sheet.GetRow(rect.Bottom).LastCellNum;

            return rect;
        }

        private static ICollection TryFillCollection(ISheet sheet, TableRect rect, Type type)
        {
            if (type.GetProperties().Count(p => p.CanWrite && (!p.GetCustomAttributes(typeof(HiddenAttribute), true).Any())) != rect.Right - rect.Left) {
                return GetEmptyCollection(type);
            }

            if (!type.GetCustomAttributes(false).Any()
                || (type.GetCustomAttributes(false)[0] is HeaderlessAttribute attr && !attr.IsHeadless)) {

                if (!CheckHeaders(sheet, rect, type)) {
                    return GetEmptyCollection(type);
                }

                ++rect.Top;
            }

            return FillModelCollection(sheet, rect, type);
        }

        private static bool CheckHeaders(ISheet sheet, TableRect rect, Type type)
        {
            var headers = GetCellHeaders(sheet, rect);
            if (null == headers) return false;

            return FillPropertyToCellIndexDictionary(type, headers);


            #region Local Function

            List<string> GetCellHeaders(ISheet sheet_, TableRect rect_)
            {
                var resSet = new List<string>();

                for (var i = rect.Left; i < rect.Right; ++i) {

                    var cellHeader = sheet_.GetRow(rect_.Top).GetCell(i).StringCellValue;

                    if (String.IsNullOrWhiteSpace(cellHeader)) return null;
                    if (cellHeader.Contains(" ")) cellHeader = cellHeader.RemoveWhitespaces();

                    resSet.Add(cellHeader.ToUpperInvariant());
                }

                return resSet;
            }

            bool FillPropertyToCellIndexDictionary(Type type_, List<string> cellHeaders)
            {
                var properties = type_.GetProperties().Where(p => p.CanWrite && (!p.GetCustomAttributes(typeof(HiddenAttribute), true).Any())).ToArray();
                if (!properties.Any()) return false;

                foreach (var propertyInfo in properties) {

                    var propertyAttr = propertyInfo.GetCustomAttributes(false)
                                                    .FirstOrDefault(a => a is HeaderAttribute) as HeaderAttribute;

                    if (null == propertyAttr) {
                        _propertyToCellColumn[propertyInfo.Name] = cellHeaders.IndexOf(propertyInfo.Name.ToUpperInvariant());
                    }
                    else {
                        _propertyToCellColumn[propertyInfo.Name] = cellHeaders.IndexOf(propertyAttr.Header.ToUpperInvariant());
                    }

                    if (-1 == _propertyToCellColumn[propertyInfo.Name]) {
                        return false;
                    }

                    _propertyToCellColumn[propertyInfo.Name] += rect.Left;
                }

                return true;
            }

            #endregion
        }

        private static ICollection FillModelCollection(SheetTable sheetTable, Type type)
        {
            ArrayList typeInstanceCollection = new ArrayList(sheetTable.Lenght);

            for (var j = sheetTable.StartCell.Row; j < sheetTable.Lenght; ++j) {

                var row = sheetTable.Sheet.GetRow(j);
                if (null == row) continue;

                object typeInstance = Activator.CreateInstance(type);

                var i = sheetTable.StartCell.Column;

                foreach (var propertyInfo in type.GetPropertyInfos()) {

                    ICell cell = row.GetCell(_propertyToCellColumn.Count == 0 ? i : _propertyToCellColumn[propertyInfo.Name]);

                    if (!SetPropertyValue(propertyInfo, cell, typeInstance)) return GetEmptyCollection(type);

                    ++i;
                }

                typeInstanceCollection.Add(typeInstance);
            }

            return typeInstanceCollection;
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static bool SetPropertyValue(PropertyInfo propertyInfo, ICell cell, object obj)
        {
            bool isSet = false;

            if (typeof(string).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, GetStringValue(cell));
                isSet = true;
            }
            else if (typeof(int).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, GetIntValue(cell));
                isSet = true;
            }
            else if (typeof(double).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, GetDoubleValue(cell));
                isSet = true;
            }
            else if (typeof(bool).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, GetBoolValue(cell));
                isSet = true;
            }
            else if (typeof(DateTime).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, GetDateTimeValue(cell));
                isSet = true;
            }

            if (!isSet) return false;

            return true;
        }

        private static string GetStringValue(ICell cell)
        {
            if (cell == null) return null;

            string stringValue = default(string);

            if (CellType.Numeric == cell.CellType) {
                try {
                    stringValue = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
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

            return stringValue;
        }

        private static int GetIntValue(ICell cell)
        {
            int intValue = default(int);
            if (cell == null) return intValue;

            if (CellType.Numeric == cell.CellType) {
                try {
                    intValue = Convert.ToInt32(cell.NumericCellValue);
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

                var cellValue = cell.StringCellValue.RemoveWhitespaces();

                if (!String.IsNullOrWhiteSpace(cellValue)) {
                    if (cellValue.ToUpperInvariant() == "Да".ToUpperInvariant()
                        || cellValue.ToUpperInvariant() == "Yes".ToUpperInvariant()) {
                        intValue = 1;
                    }
                    else {
                        cellValue = cellValue.Replace(',', '.');

                        try {
                            intValue = Convert.ToInt32(Double.Parse(cellValue));
                        }
                        catch {
                        }
                    }
                }
            }

            return intValue;
        }

        private static double GetDoubleValue(ICell cell)
        {
            double doubleValue = default(double);
            if (cell == null) return doubleValue;

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

                var cellValue = cell.StringCellValue.RemoveWhitespaces();

                if (!String.IsNullOrWhiteSpace(cellValue)) {

                    if (cellValue.ToUpperInvariant() == "Да".ToUpperInvariant()
                        || cellValue.ToUpperInvariant() == "Yes".ToUpperInvariant()) {

                        doubleValue = 1.0;
                    }
                    else {
                        cellValue = cellValue.Replace(',', '.');

                        try {
                            doubleValue = Double.Parse(cellValue);
                        }
                        catch {
                        }
                    }
                }
            }

            return doubleValue;
        }

        private static bool GetBoolValue(ICell cell)
        {
            bool boolValue = default(bool);
            if (cell == null) return boolValue;

            if (CellType.Numeric == cell.CellType) {

                try {
                    boolValue = !cell.NumericCellValue.Equals(0.0);
                }
                catch (OverflowException) {
                    boolValue = false;
                }
            }
            else if (CellType.Boolean == cell.CellType) {

                boolValue = cell.BooleanCellValue;
            }
            else if (CellType.String == cell.CellType) {

                var cellValue = cell.StringCellValue.RemoveWhitespaces();

                if (!String.IsNullOrWhiteSpace(cellValue)) {

                    if (cellValue.ToUpperInvariant() == "Да".ToUpperInvariant()
                        || cellValue.ToUpperInvariant() == "Yes".ToUpperInvariant()) {

                        boolValue = true;
                    }
                }
            }

            return boolValue;
        }

        private static DateTime GetDateTimeValue(ICell cell)
        {
            DateTime dateTimeValue = default(DateTime);
            if (cell == null) return dateTimeValue;

            try {
                if (CellType.Numeric == cell.CellType) {

                    dateTimeValue = new DateTime(Convert.ToInt64(cell.NumericCellValue));
                }
                else if (CellType.String == cell.CellType) {

                    int days = Int32.Parse (cell.StringCellValue);
                    dateTimeValue = new DateTime(1900, 1, 1).AddDays (days - 2);
                }
            }
            catch {
            }

            return dateTimeValue;
        }

        private static ICollection GetEmptyCollection(Type type)

        {
            Type t = typeof(List<>);
            var constr = t.MakeGenericType(type);
            return (ICollection)Activator.CreateInstance(constr);
        }

        #endregion
    }
}
