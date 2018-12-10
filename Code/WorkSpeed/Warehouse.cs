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

namespace WorkSpeed
{
    public class Warehouse : WarehouseEntities, IWarehouse
    {
        private readonly ITypeRepository _typeRepository;


        public Warehouse()
        {
            _typeRepository = new TypeRepository();
            AddTypesToRepository (_typeRepository);
        }

        public IWarehouseEntities NewData { get; }

        public async Task ImportAsync (string fileName)
        {
            await Task.Factory.StartNew (() => Import (fileName), TaskCreationOptions.LongRunning);
        }

        private void Import (string fileName)
        {
            var sheetTable = ExcelImporter.ImportData (fileName, 0);
            var type = _typeRepository.GetType (sheetTable.Headers, new[] {typeof (HeaderAttribute)},
                                                                    new[] {typeof (HiddenAttribute)});

            FillNewData (ExcelImporter.ToCollection (sheetTable, type), type);

        }

        private void FillNewData (IEnumerable data, Type type)
        {
                NewData.Add (data.Cast<ImportModel>());
        }

        private void AddTypesToRepository (ITypeRepository repository)
        {

        }
    }
}
