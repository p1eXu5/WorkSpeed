using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public sealed class Importer
    {
        private readonly ITypeRepository _typeRepository;

        public Importer(ITypeRepository typeRepository)
        {
            _typeRepository = typeRepository ?? throw new ArgumentNullException();
        }


        #region Methods

        /// <summary>
        /// Register a custom data importer.
        /// </summary>
        /// <typeparam name="TConcreteImporter"><see cref="IConcreteImporter"/></typeparam>
        /// <param name="importer">Instance of concrete importer.</param>
        /// 
        public void RegisterImporter<TDataImporter>(TDataImporter importer) where TDataImporter : IDataImporter
        {
            if (importer == null) {
                throw new NullReferenceException($"{nameof(importer)} can't be null");
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
            throw new NotImplementedException();
        }

        #endregion
        
    }
}
