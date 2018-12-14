using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using static Helpers.StringExtensions;
using static Helpers.EasyTypeBuilder;

namespace NpoiExcel
{
    public static class ExcelImporter
    {

        public static SheetTable ImportData (string fileName, int sheetIndex)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {

                return ImportData(stream, sheetIndex);
            }
        }

        public static SheetTable ImportData (Stream stream, int sheetIndex)
        {
            if (sheetIndex < 0) throw new ArgumentException("sheetIndex must be equal or greater than zero.", nameof(sheetIndex));

            // Load sheetTable:
            ISheet sheet;

            try {
                sheet = GetSheet(stream, sheetIndex);
            }
            catch {
                throw new FileFormatException($"{stream} has invalid data format or has no sheetTable with {sheetIndex} index.");
            }

            return new SheetTable(sheet);
        }


        public static ICollection ImportData (string fileName, Type type, int sheetIndex)
        {
            using (var stream = new FileStream (fileName, FileMode.Open, FileAccess.Read)) {

                return ImportData (stream, type, sheetIndex);
            }
        }

        public static ICollection ImportData (Stream source, Type type, int sheetIndex)
        {
            CheckType (type);

            SheetTable sheetTable;

            try {
                sheetTable = ImportData (source, sheetIndex);
            }
            catch (ArgumentException) {
                return GetEmptyCollection (type);
            }

            // Load headers map:
            var headersMap = GetHeaderMap (type, sheetTable);
            if (!headersMap.Keys.Any()) return GetEmptyCollection (type);

            return FillModelCollection(sheetTable, type, headersMap);
        }



        public static ICollection ToCollection (SheetTable sheetTable, Type type, ITypeConverter converter)
        {

            return null;
        }



        private static void CheckType (Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type), "type can't be null.");

            if (type.GetConstructors().Count(c => c.IsPublic && c.GetParameters().Length == 0) == 0) {
                throw new TypeAccessException($"{type} has no public parameterless constructor!");
            }
        }


        private static ISheet GetSheet (Stream stream, int sheetIndex)
        {
            IWorkbook book;

            try { 
                book = new XSSFWorkbook(stream);
            }
            catch(Exception ex) {

                Debug.WriteLine (ex.Message);
                book = new HSSFWorkbook(stream);
            }

            return book.GetSheetAt(sheetIndex);
        }

        private static Dictionary<string, int> GetHeaderMap (Type type, SheetTable sheetTable)
        {
            var map = new Dictionary<string, int>();
            var propertyTuples = type.IsHeaderless() ? GetPropertyNames (type) : GetPropertyHeaders (type);

            if (!propertyTuples.Any()) return map;

            for (var i = 0; i < sheetTable.ColumnCount; ++i) {

                foreach (var tuple in propertyTuples) {

                    if (tuple.headers.Contains (sheetTable.GetNormalizedHeaderAt(i))) {

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

        private static ICollection FillModelCollection(SheetTable sheetTable, Type type, Dictionary<string, int> headersMap)
        {
            ArrayList typeInstanceCollection = new ArrayList(sheetTable.RowCount);

            for (var j = 0; j < sheetTable.RowCount; ++j) {

                object typeInstance = Activator.CreateInstance(type);

                var i = 0;

                foreach (var propertyInfo in type.GetPropertyInfos()) {

                    var cell = sheetTable[j, headersMap[propertyInfo.Name]];

                    if (!SetPropertyValue(propertyInfo, cell, typeInstance)) return GetEmptyCollection(type);

                    ++i;
                }

                typeInstanceCollection.Add(typeInstance);
            }

            return typeInstanceCollection;
        }


        private static bool IsHeaderless (this Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns public writable properties with no HiddenAttribute
        /// </summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <returns><see cref="Array"/></returns>
        private static HashSet<(string[] headers, string name)> GetPropertyNames (Type type)
        {
            throw new NotImplementedException();
        }

        private static HashSet<(string[] headers, string name)> GetPropertyHeaders (Type type)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<PropertyInfo> GetPropertyInfos (this Type type)
        {
            throw new NotImplementedException();
        }


        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        private static bool SetPropertyValue(PropertyInfo propertyInfo, CellValue cell, object obj)
        {
            bool isSet = false;

            if (typeof(string).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, cell);
                isSet = true;
            }
            else if (typeof(int).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, cell);
                isSet = true;
            }
            else if (typeof(double).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, cell);
                isSet = true;
            }
            else if (typeof(bool).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, cell);
                isSet = true;
            }
            else if (typeof(DateTime).FullName == propertyInfo.PropertyType.FullName) {

                obj.GetType().GetProperty(propertyInfo.Name).SetValue(obj, cell);
                isSet = true;
            }

            if (!isSet) return false;

            return true;
        }
    }
}
