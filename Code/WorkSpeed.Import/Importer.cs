using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using WorkSpeed.Import.FileImporters;
using WorkSpeed.Import.Models;
using WorkSpeed.Import.Models.ImportModels;

namespace WorkSpeed.Import
{
    public sealed class Importer : ITypeRepository
    {
        private readonly Dictionary<string, IFileImporter> _map = new Dictionary<string, IFileImporter>();

        private readonly HashSet<ImportedAction> _actions = new HashSet<ImportedAction>();

        private readonly Dictionary<string, string> _employees = new Dictionary<string, string>();

        private readonly HashSet<ImportedProduct> _products;
        private readonly Dictionary<int, ProductMgh> _productMghs = new Dictionary<int, ProductMgh>();

        private readonly HashSet<ImportedOperation> _operations = new HashSet<ImportedOperation>();
        private readonly HashSet<string> _addresses = new HashSet<string>();
        private readonly Dictionary<string, string> _documents = new Dictionary<string, string>();

        public Importer()
        {
        }

        public IEnumerable<string> FileExtensions => _map.Keys;

        public (DateTime startDate, DateTime endDate)? Period { get; private set; }

        public IEnumerable<KeyValuePair<string, string>> Employees => _employees;

        public IEnumerable<ImportedProduct> Products => _products;
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
                throw new ArgumentNullException($"{nameof(fileImporter)} can't be null");
            }

            if (fileImporter.FileExtensions == null || !fileImporter.FileExtensions.Any()) {
                throw new ArgumentException ("IFileImporter instance does not have extensions", nameof(fileImporter.FileExtensions));
            }

            var extensions = fileImporter.FileExtensions.Where (e => !String.IsNullOrWhiteSpace(e) && e[0] == '.' && !e.HasWhitespaces()).ToArray();

            if (0 == extensions.Length) {
                throw new ArgumentException("IFileImporter instance does not have valid extensions", nameof(fileImporter.FileExtensions));
            }

            foreach (var extension in extensions) {
                _map[extension] = fileImporter;
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
        public bool ImportData(string fileName)
        {
            var ext = Path.GetExtension (fileName);

            if (ext != null && _map.ContainsKey (ext)) {
                _map[ext].ImportData (fileName, this);
                return true;
            }

            return false;
        }

        public IEnumerable<ImportedAction> GetActions (HashSet<string> operations)
        {
            HashSet<byte> operationIndexes = _operations.GetIndexes (operations);
            return _actions.Where (a => operationIndexes.Contains (a.OperationId));
        }

        #endregion
        
    }
}
