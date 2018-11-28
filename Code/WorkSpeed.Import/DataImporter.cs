using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WorkSpeed.Import
{
    public sealed class DataImporter : IDataImporter
    {
        private readonly Dictionary<string,Func<string,Type,ICollection>> _strategies = new Dictionary<string, Func<string,Type,ICollection>>();

        public DataImporter()
        {
            RegisterImporter (ExcelImporter.ExcelImporterInstance);
        }


        #region Methods

        /// <summary>
        /// Register a custom data importer.
        /// </summary>
        /// <typeparam name="TConcreteImporter"><see cref="IConcreteImporter"/></typeparam>
        /// <param name="importer">Instance of concrete importer.</param>
        /// 
        public void RegisterImporter<TConcreteImporter>(TConcreteImporter importer) where TConcreteImporter : IConcreteImporter
        {
            if (importer == null) {
                throw new NullReferenceException($"{nameof(importer)} can't be null");
            }

            // TODO костыль
            foreach (string fileExtension in importer.FileExtensions) {
                
                _strategies[fileExtension] = importer.ImportData;
            }
        }


        /// <summary>
        /// Imports data from file in asynchronous manner.
        /// </summary>
        /// <typeparam name="TModelType">
        ///     Model type with annotated or not annotated public properties
        ///     that corresponds data table in specified file.
        /// </typeparam>
        /// <param name="fileName">File name.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        /// 
        public IEnumerable<TModelType> ImportDataAsync<TModelType> (string fileName) where TModelType : new()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Imports data from file.
        /// </summary>
        /// <typeparam name="TModelType">
        ///     Model type with annotated or not annotated public properties
        ///     that corresponds data table in specified file.
        /// </typeparam>
        /// <param name="fileName">File name.</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        /// 
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
        public IEnumerable<TModelType> ImportData<TModelType>(string fileName) where TModelType : new()
        {
            if (!File.Exists(fileName)) { throw new FileNotFoundException(); }
            
            if (!_strategies.ContainsKey(Path.GetExtension(fileName))) {
                throw new ArgumentException("The source does not handled");
            }

            var typeProperties = typeof(TModelType).GetProperties();
            if (0 == typeProperties.Length) { throw new TypeAccessException(@"Passed type does not have public properties"); }

            return (IEnumerable<TModelType>)_strategies[Path.GetExtension(fileName)].Invoke(fileName, typeof(TModelType));
        }

        #endregion
        
    }
}
