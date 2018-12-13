using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using NpoiExcel;
using WorkSpeed.Attributes;
using WorkSpeed;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed
{
    public class Warehouse : IWarehouse
    {
        private readonly ITypeRepository _typeRepository;
        private readonly ProductivityObservableCollection _productivities;

        public Warehouse()
        {
            _typeRepository = new ImportActionTypeRepository();
            AddTypesToRepository (_typeRepository);

            _productivities = new ProductivityObservableCollection();
        }

        /// <summary>
        /// Entities that don't contained in DB.
        /// </summary>
        public IWarehouseEntities NewData { get; }

        public async Task<bool> ImportAsync (string fileName)
        {
            return await Task<bool>.Factory.StartNew (() => Import (fileName), TaskCreationOptions.LongRunning);
        }

        private bool Import (string fileName)
        {
            var sheetTable = ExcelImporter.ImportData (fileName, 0);
            var importCollection = _typeRepository.GetTypeCollection (sheetTable, new[] {typeof (HeaderAttribute)},
                                                                                  new[] {typeof (HiddenAttribute)});

            if (importCollection == null || importCollection.Count == 0) return false;

            switch (importCollection[0]) {

                case ActionImportModel action:

                    FillProductivityCollection (importCollection.Cast<ActionImportModel>());
                    //CheckNewData (_productivities);
                    break;
            }

            return true;
        }

        private void FillProductivityCollection (IEnumerable<ActionImportModel> actions)
        {
            if (_productivities.Any()) {
                _productivities.Clear();
            }

            foreach (var action in actions) {
                _productivities.Add (action);
            }
        }

        private void AddTypesToRepository (ITypeRepository repository)
        {

        }
    }
}
