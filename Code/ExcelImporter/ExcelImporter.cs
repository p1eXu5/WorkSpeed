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
using static Helpers.StringExtensions;
using static Helpers.EasyTypeBuilder;

namespace ExcelImporter
{
    public static class ExcelImporter
    {
        public static ICollection ImportData (string path, Type type, int sheetIndex)
        {
            using (var stream = new FileStream (path, FileMode.Open, FileAccess.Read)) {

                return ImportData (stream, type, sheetIndex);
            }
        }

        public static ICollection ImportData (Stream source, Type type, int sheetIndex)
        {
            if (type == null) throw new ArgumentNullException (nameof(type), "type can't be null.");

            if (type.GetConstructors().Count(c => c.IsPublic && c.GetParameters().Length == 0) == 0) {
                throw new TypeAccessException($"{type} has no public parameterless constructor!");
            }

            if (sheetIndex < 0) throw new ArgumentException ("sheetIndex must be equal or greater than zero.", nameof(sheetIndex));

            // Load sheetTable:
            ISheet sheet;

            try {
                sheet = GetSheet (source, sheetIndex);
            }
            catch {
                throw new FileFormatException($"{source} has invalid data format or has no sheetTable with {sheetIndex} index!");
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

            return FillModelCollection(sheetTable, type, headersMap);
        }

        private static ISheet GetSheet (Stream stream, int sheetIndex)
        {
            IWorkbook book;

            try { 
                book = new HSSFWorkbook(stream);
            }
            catch {
                book = new XSSFWorkbook(stream);
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
            return type.GetCustomAttributes (typeof (HeaderlessAttribute)).Any();
        }

        /// <summary>
        /// Returns public writable properties with no HiddenAttribute
        /// </summary>
        /// <param name="type"><see cref="Type"/></param>
        /// <returns><see cref="Array"/></returns>
        private static HashSet<(string[] headers, string name)> GetPropertyNames (Type type)
        {
            return  new HashSet<(string[] headers, string name)> 
                        (
                                type.GetProperties()
                                    .Where (p => p.CanWrite && !p.GetCustomAttributes(typeof(HiddenAttribute)).Any())
                                    .Select (p => (new [] {""}, p.Name))
                        );
        }

        private static HashSet<(string[] headers, string name)> GetPropertyHeaders (Type type)
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

        private static IEnumerable<PropertyInfo> GetPropertyInfos (this Type type)
        {
            return type.GetProperties()
                       .Where (p => p.CanWrite && !p.GetCustomAttributes (typeof(HiddenAttribute)).Any());
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
