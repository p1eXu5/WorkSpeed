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
    public sealed class Importer : ITypeRepository
    {

        public Importer()
        {
        }


        #region Methods

        /// <summary>
        /// Register a custom data importer.
        /// </summary>
        /// <param name="fileImporter">Instance of concrete importer.</param>
        /// 
        public void RegisterFileImporter(IFileImporter fileImporter)
        {
            if (fileImporter == null) {
                throw new NullReferenceException($"{nameof(fileImporter)} can't be null");
            }
        }


        public IEnumerable<string> GetFileExtensions()
        {
            throw new NotImplementedException();
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
        public bool ImportData(string fileName)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}
