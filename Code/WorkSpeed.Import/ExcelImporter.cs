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
        public static ExcelImporter ExcelImporterInstance => _excelImporter ?? (_excelImporter = new ExcelImporter());

        public string FileExtension { get; } = ".xlsx";
        public Func<string, Type, ICollection> ImportData { get; } = ImportDataFromExcel;


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

        private static TableRect? GetFirstCell(ISheet sheet, int propertyCount)
        {
            TableRect rect;
            rect.Top = sheet.FirstRowNum;
            rect.Bottom = sheet.LastRowNum;

            if (rect.Bottom < rect.Top) throw new ArgumentException();


            rect.Left = sheet.GetRow (rect.Top)?.FirstCellNum ?? -1;
            if (rect.Left < 0) return null;

            rect.Right = sheet.GetRow (rect.Bottom).LastCellNum;

            if (rect.Left > rect.Right) throw new ArgumentException();

            if (sheet.GetRow (rect.Top).GetCell (rect.Left).CellType == CellType.Blank) {

                if (!FindLeftCell()) return null;
            } 

            if (sheet.GetRow (rect.Bottom).GetCell (rect.Right - 1).CellType == CellType.Blank) {
                FindRightCell();
            }
            
            if (propertyCount != (rect.Right - rect.Left)) {
                return null;
            }

            for (var i = rect.Top + 1; i <= rect.Bottom; ++i) {

                var row = sheet.GetRow (i);
                if (null == row) continue;

                if (rect.Left > row.FirstCellNum) {
                    rect.Left = row.FirstCellNum;
                }

                if (rect.Right < row.LastCellNum) {
                    rect.Right = row.LastCellNum;
                }
            }

            // var forTest = sheet.GetRow (2)?.GetCell (2) ?? null;

            return rect;

            bool FindLeftCell()
            {
                int j = rect.Top;

                int i = rect.Left + 1;
                int iend = sheet.GetRow (rect.Top).LastCellNum;

                while (sheet.GetRow (j)?.GetCell (i) == null || sheet.GetRow (j).GetCell (i).CellType == CellType.Blank) {

                    if (i >= iend) {
                        do {
                            ++j;
                            if (j > rect.Bottom) {
                                break;
                            }
                        } while (sheet.GetRow (j) == null && j <= rect.Bottom);

                        if (null == sheet.GetRow (j)) {
                            return false;
                        }

                        i = sheet.GetRow (j).FirstCellNum;
                        iend = sheet.GetRow (j).LastCellNum;
                    }
                    else {
                        ++i;
                    }
                }

                rect.Left = i;
                rect.Top = j;

                return true;
            }

            void FindRightCell()
            {
                int j = rect.Bottom;

                int i = rect.Right - 2;
                int iend = sheet.GetRow (rect.Bottom).FirstCellNum;

                while (sheet.GetRow (j)?.GetCell (i) == null || sheet.GetRow (j).GetCell (i).CellType == CellType.Blank) {

                    if (i == iend) {
                        do {
                            --j;
                        } while (sheet.GetRow (j) == null);

                        i = sheet.GetRow (j).LastCellNum;
                        iend = sheet.GetRow (j).FirstCellNum;
                    }
                    else {
                        --i;
                    }
                }

                rect.Right = i;
                rect.Bottom = j;
            }
        }

        private static ICollection GetEmptyCollection(Type type)
        {
            Type t = typeof(List<>);
            var constr = t.MakeGenericType (type);
            return (ICollection)Activator.CreateInstance (constr);
        }

        private static ICollection TryFillCollection (ISheet sheet, TableRect rect, Type type)
        {
            if (type.GetCustomAttributes (false)[0] is HeadlessAttribute attr && !attr.IsHeadless) {

                if (!CheckHeaders()) {
                    return GetEmptyCollection (type);
                }
            }

            return FillModelCollection();
            

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
        }
    }
}
