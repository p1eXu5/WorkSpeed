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
            _typeRepository = new TypeRepository();
            AddTypesToRepository (_typeRepository);

            _productivities = new ProductivityObservableCollection();
        }

        private void AddTypesToRepository ( ITypeRepository repo )
        {
            repo.RegisterType< ProductivityImportModel >();
            repo.RegisterType< GatheringImportModel >();
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
            var mappedType = _typeRepository.GetTypeWithMap ( sheetTable );

            FillProductivityCollection(
                ExcelImporter.GetEnumerable( sheetTable, mappedType,
                                             new ImportModelConverter( new ImportModelVisiter() ) )
            );

            return true;
        }

        private void FillProductivityCollection (IEnumerable<EmployeeAction> actions)
        {

        }
    }
}
