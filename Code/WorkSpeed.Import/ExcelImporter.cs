using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WorkSpeed.Import.Attributes;
using WorkSpeed.Import.Models;

namespace WorkSpeed.Import
{
    /// <summary>
    /// Singletone.
    /// </summary>
    [SuppressMessage ("ReSharper", "EmptyGeneralCatchClause")]
    public class ExcelImporter : IDataImporter
    {
        private const string XLS_FILE = ".xls";
        private const string XLSX_FILE = ".xlsx";

        protected readonly Dictionary<string,int> PropertyToCellColumn = new Dictionary<string, int>();

        protected ExcelImporter() {}

        #region Properties

        public static ReadOnlyHashSet<string> FileExtensions { get; } = new ReadOnlyHashSet<string> { XLS_FILE, XLSX_FILE };

        #endregion
        
        #region Methods

        public IEnumerable<string> GetFileExtensions() => FileExtensions;

        IEnumerable<ProductivityImportModel> IDataImporter.ImportData (string fileName, ITypeRepository typeRepository) => ImportData (fileName, typeRepository);


        public static IEnumerable<ProductivityImportModel> ImportData (string fileName, ITypeRepository typeRepository)
        {
            if (!File.Exists(fileName)) throw new ArgumentException("File doesn't exist", nameof(fileName));
            if (typeRepository == null) throw new ArgumentNullException(nameof(typeRepository), "typeRepository can not be null");

            ISheet sheet = GetSheetAtZeroIndex (fileName);
            if (null == sheet) return new ProductivityImportModel[0];

            (Type type, int[] columns) = GetMap (sheet, typeRepository);

            if (type == null) throw new ArgumentException("Type repository doesn't contain correspoding type", nameof(typeRepository));

            return FillModelCollection (type, columns);
        }

        public static IEnumerable<ProductivityImportModel> ImportData (string fileName, Type type)
        {
            if (!File.Exists(fileName)) throw new ArgumentException("File doesn't contain any sheets", nameof(fileName));
            if (type == null) throw new ArgumentNullException(nameof(type), "Type can not be null");

            ISheet sheet = GetSheetAtZeroIndex (fileName);
            if (null == sheet) return new ProductivityImportModel[0];

            int[] columns = GetMap (sheet, type);

            return FillModelCollection (type, columns);
        }

        private static ISheet GetSheetAtZeroIndex (string fileName)
        {
            using (Stream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                try {

                    IWorkbook book;

                    switch (Path.GetExtension(fileName)) {
                        case XLSX_FILE:
                            book = new XSSFWorkbook (stream);
                            break;
                        case XLS_FILE:
                            book = new HSSFWorkbook (stream);
                            break;
                        default:
                            return null;
                    }

                    return book.GetSheetAt (0);
                }
                catch {
                    return null;
                }
            }
        }

        private static (Type, int[]) GetMap (ISheet sheet, ITypeRepository typeRepository)
        {
            throw new NotImplementedException();
        }

        private static int[] GetMap (ISheet sheet, Type type)
        {
            throw new NotImplementedException();
        }

        protected static ProductivityImportModel[] FillModelCollection (Type type, int[] columns)
        {
            throw new NotImplementedException();
        }


        //public static ICollection ImportDataFromExcel(string fileName, Type type)
        //{
        //    if (PropertyToCellColumn.Any()) PropertyToCellColumn.Clear();

        //    if (type.GetConstructors().Count (c => c.IsPublic && c.GetParameters().Length == 0) == 0) {
        //        return GetEmptyCollection(type);
        //    }

        //    var fileExtension = Path.GetExtension (fileName);
        //    if (!Instance.FileExtensions.Contains (fileExtension)) return GetEmptyCollection(type);

        //    using (Stream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

        //        try {

        //            IWorkbook book;

        //            if (XLS_FILE == fileExtension) {
        //                book = new HSSFWorkbook(stream);
        //            }
        //            else {
        //                 book = new XSSFWorkbook (stream);
        //            }

        //            ISheet sheet = book.GetSheetAt (0);

        //            var tableRect = GetFirstCell (sheet);

        //            if (null == tableRect) {
        //                return GetEmptyCollection (type);
        //            }

        //            return TryFillCollection (sheet, (TableRect)tableRect, type) ?? GetEmptyCollection (type);
        //        }
        //        catch (ZipException) {

        //            return GetEmptyCollection (type);
        //        }
        //    }
        //}


        /// <summary>
        /// Returns data area in Excel table.
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        protected static TableRect? GetFirstCell(ISheet sheet)
        {
            TableRect tableRect = GetRect(sheet);

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


            #endregion
        }

        protected static TableRect GetRect(ISheet sheet)
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

        protected static ICollection TryFillCollection (ISheet sheet, TableRect rect, Type type)
        {
            if (type.GetProperties ().Count(p => p.CanWrite) != rect.Right - rect.Left) {
                return GetEmptyCollection (type);
            }

            if (   !type.GetCustomAttributes (false).Any() 
                || (type.GetCustomAttributes (false)[0] is HeadlessAttribute attr && !attr.IsHeadless)) {

                if (!CheckHeaders(sheet, rect, type)) {
                    return GetEmptyCollection (type);
                }

                ++rect.Top;
            }

            return null; //FillModelCollection(sheet, rect, type);
        }

        protected static bool CheckHeaders (ISheet sheet, TableRect rect, Type type)
        {
            var headers = GetCellHeaders (sheet, rect);
            if (null == headers) return false;

            return false;//FillPropertyToCellIndexDictionary(type, headers);


            #region Local Function

            List<string> GetCellHeaders(ISheet sheet_, TableRect rect_)
            {
                var resSet = new List<string>();

                for (var i = rect.Left; i < rect.Right; ++i) {

                    var cellHeader = sheet_.GetRow (rect_.Top).GetCell (i).StringCellValue;

                    if (String.IsNullOrWhiteSpace (cellHeader)) return null;
                    if (cellHeader.Contains (" ")) cellHeader = cellHeader.RemoveWhitespaces();

                    resSet.Add (cellHeader.ToUpperInvariant());
                }

                return resSet;
            }

            //bool FillPropertyToCellIndexDictionary (Type type_, List<string> cellHeaders)
            //{
            //    var properties = type_.GetProperties().Where (p => p.CanWrite).ToArray();
            //    if (!properties.Any()) return false;

            //    foreach (var propertyInfo in properties) {

            //        var propertyAttr = propertyInfo.GetCustomAttributes (false)
            //                                        .FirstOrDefault (a => a is HeaderAttribute) as HeaderAttribute;

            //        if (null == propertyAttr) {
            //            PropertyToCellColumn[propertyInfo.Name] = cellHeaders.IndexOf (propertyInfo.Name.ToUpperInvariant());
            //        }
            //        else{
            //            PropertyToCellColumn[propertyInfo.Name] = cellHeaders.IndexOf (propertyAttr.Header.ToUpperInvariant());
            //        }

            //        if (-1 == PropertyToCellColumn[propertyInfo.Name]) {
            //            return false;
            //        }

            //        PropertyToCellColumn[propertyInfo.Name] += rect.Left;
            //    }

            //    return true;
            //}

            #endregion
        }

        //private static ICollection FillModelCollection(ISheet sheet, TableRect rect, Type type)
        //{
        //    ArrayList typeInstanceCollection = new ArrayList(rect.Bottom - rect.Top + 1);

        //    for (var j = rect.Top; j <= rect.Bottom; ++j) {

        //        var row = sheet.GetRow (j);
        //        if (null == row) continue;

        //        object typeInstance = Activator.CreateInstance (type);

        //        var i = rect.Left;

        //        foreach (var propertyInfo in type.GetProperties().Where (p => p.CanWrite)) {

        //            ICell cell = row.GetCell (PropertyToCellColumn.Count == 0 ? i : PropertyToCellColumn[propertyInfo.Name]);

        //            if (!SetPropertyValue(propertyInfo, cell, typeInstance)) return GetEmptyCollection(type);

        //            ++i;
        //        }

        //        typeInstanceCollection.Add (typeInstance);
        //    }

        //    return typeInstanceCollection;
        //}

        [SuppressMessage ("ReSharper", "PossibleNullReferenceException")]
        private static bool SetPropertyValue (PropertyInfo propertyInfo, ICell cell, object obj)
        {
            bool isSet = false;

            if (typeof (string).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, GetStringValue(cell));
                isSet = true;
            }
            else if (typeof (int).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, GetIntValue(cell));
                isSet = true;
            }
            else if (typeof (double).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, GetDoubleValue(cell));
                isSet = true;
            }
            else if (typeof (bool).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, GetBoolValue(cell));
                isSet = true;
            }
            else if (typeof(DateTime).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, GetDateTimeValue(cell));
                isSet = true;
            }

            if (!isSet) return false;
            
            return true;
        }

        protected static string GetStringValue (ICell cell)
        {
            if (cell == null) return null;

            string stringValue = default(string);

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

            return stringValue;
        }

        protected static int GetIntValue (ICell cell)
        {
            int intValue = default(int);
            if (cell == null) return intValue;

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

            return intValue;
        }

        protected static double GetDoubleValue (ICell cell)
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

            return doubleValue;
        }

        protected static bool GetBoolValue (ICell cell)
        {
            bool boolValue = default(bool);
            if (cell == null) return boolValue;

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

            return boolValue;
        }

        protected static DateTime GetDateTimeValue(ICell cell)
        {
            DateTime dateTimeValue = default(DateTime);
            if (cell == null) return dateTimeValue;

            try {
                if (CellType.Numeric == cell.CellType) {
                    dateTimeValue = new DateTime(Convert.ToInt64 (cell.NumericCellValue));
                }
                else if (CellType.String == cell.CellType) {
                    dateTimeValue = DateTime.Parse(cell.StringCellValue);
                }
            }
            catch {
            }

            return dateTimeValue;
        }

        protected static ICollection GetEmptyCollection(Type type)

        {
            Type t = typeof(List<>);
            var constr = t.MakeGenericType (type);
            return (ICollection)Activator.CreateInstance (constr);
        }

        #endregion
    }
}
