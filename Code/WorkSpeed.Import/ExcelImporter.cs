using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            using (Stream stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                try {
                    IWorkbook book = new XSSFWorkbook (stream);
                    ISheet sheet = book.GetSheetAt (0);

                    var tableRect = GetFirstCell (sheet, type.GetProperties().Length);

                    if (null == tableRect) {
                        return GetEmptyCollection (type);
                    }

                    return TryFillCollection (sheet, (TableRect)tableRect, type);
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
        /// <param name="propertyCount"></param>
        /// <returns></returns>
        private static TableRect? GetFirstCell(ISheet sheet, int propertyCount)
        {
            TableRect tableRect = GetRect();

            var j = tableRect.Top;
            var i = tableRect.Left;

            if (tableRect.Top == tableRect.Bottom && tableRect.Left == tableRect.Right - 1) {

                if (sheet.GetRow (tableRect.Top).GetCell (tableRect.Left).CellType == CellType.Blank) {
                    return null;
                }

                return tableRect;
            }
            else if (tableRect.Left == tableRect.Right - 1) {

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
            else {
                do {
                    IRow row = sheet.GetRow (j);

                    if (tableRect.Left > 0) {
                        if (tableRect.Left > row.FirstCellNum) {
                            tableRect.Left = row.FirstCellNum;
                        }
                    }

                    if (tableRect.Right < row.LastCellNum) {
                        tableRect.Right = row.LastCellNum;
                    }

                    ++j;
                } while (j <= tableRect.Bottom);

                return tableRect;
            }

            // var forTest = sheet.GetRow (2)?.GetCell (2) ?? null;

            #region Functions

            TableRect GetRect()
            {
                TableRect rect;

                rect.Top = sheet.FirstRowNum;
                rect.Bottom = sheet.LastRowNum;

                if (rect.Bottom < rect.Top) throw new ArgumentException();

                rect.Left = sheet.GetRow (rect.Top).FirstCellNum;
                rect.Right = sheet.GetRow (rect.Bottom).LastCellNum;

                return rect;
            }

            #endregion
        }

        private static ICollection TryFillCollection (ISheet sheet, TableRect rect, Type type)
        {
            if (!type.GetCustomAttributes (false).Any() || (type.GetCustomAttributes (false)[0] is HeadlessAttribute attr && !attr.IsHeadless)) {
                if (!CheckHeaders()) {
                    return GetEmptyCollection (type);
                }

                ++rect.Top;
            }

            return FillModelCollection();

            #region Functions

            ArrayList FillModelCollection()
            {
                object obj = Activator.CreateInstance (type);
                ArrayList list = new ArrayList(rect.Bottom - rect.Top + 1);

                for (var j = rect.Top; j <= rect.Bottom; ++j) {

                    var i = rect.Left;

                    foreach (var propertyInfo in type.GetProperties()) {

                        if (propertyInfo.SetMethod == null) continue;

                        if (propertyInfo.PropertyType.FullName == typeof(string).FullName) {

                            // ReSharper disable once PossibleNullReferenceException
                            obj.GetType().GetProperty (propertyInfo.Name).SetValue (obj, sheet.GetRow (j)?.GetCell (i)?.StringCellValue ?? String.Empty);
                            list.Add (obj);

                            obj = Activator.CreateInstance (type);
                            ++i;
                        }
                    }
                }

                return list;
            }

            bool CheckHeaders ()
            {
                var i = rect.Left;

                foreach (var propertyInfo in type.GetProperties()) {

                    if (((HeaderAttribute) propertyInfo
                                            .GetCustomAttributes (false)
                                            .Single (a => a is HeaderAttribute))
                                            .Header != sheet.GetRow (rect.Top).GetCell (i).StringCellValue) {
                        return false;
                    }

                    ++i;
                }

                return true;
            }

            #endregion
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
