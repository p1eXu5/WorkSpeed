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
using WorkSpeed.Import.Models;

namespace WorkSpeed.Import
{
    public sealed class Importer : ITypeRepository
    {
        private readonly HashSet<ImportedAction> _actions;

        private readonly Dictionary<string, string> _employees = new Dictionary<string, string>();

        private readonly HashSet<ImportedProduct> _products;
        private readonly Dictionary<int, ProductMgh> _productMghs = new Dictionary<int, ProductMgh>();

        private readonly HashSet<ImportedOperation> _operations = new HashSet<ImportedOperation>();
        private readonly HashSet<string> _addresses = new HashSet<string>();
        private readonly Dictionary<string, string> _documents = new Dictionary<string, string>();

        public Importer()
        {
        }

        public (DateTime startDate, DateTime endDate) Period { get; private set; }

        public IEnumerable<KeyValuePair<string, string>> Employees => _employees;

        public IEnumerable<KeyValuePair<int, string>> Products => _products;
        public IEnumerable<KeyValuePair<int, ProductMgh>> ProductMghs => _productMghs;

        public IEnumerable<ImportedOperation> Operations => _operations;
        public IEnumerable<string> Addresses => _addresses;

        public IEnumerable<KeyValuePair<string, string>> Documents => _documents;

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
