using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.DataContexts.ImportServiceExtensions;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts
{
    public class ImportService : Service, IImportService
    {
        private readonly ITypeRepository _typeRepository;

        public ImportService ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext )
        {
            _typeRepository = typeRepository;
        }

        public Task ImportFromXlsxAsync ( string fileName, IProgress< (int, string) > progress )
        {
            var task = new Task( () => ImportFromXlsx( fileName, progress ) );
            task.Start();

            return task;
        }

        public void ImportFromXlsx ( string fileName, IProgress< (int, string) > progress )
        {
            var table = ExcelImporter.GetSheetTable( fileName );
            var typeMap = _typeRepository.GetTypeAndPropertyMap( table );

            if ( null == typeMap.type ) {

                progress.Report( (-1, @"Не удалось прочитать файл. Файл либо открыт в другой программе, " 
                                      + @"либо содержит таблицу, тип которой определить не удалось.") );

                return;
            }

            var data = ExcelImporter.GetDataFromTable( table, typeMap.propertyMap, new ImportModelConverter() );
            if ( data.Any() ) {
                StoreData( ( dynamic )data );
            }
        }

        private void StoreData ( IEnumerable< Product > data )
        {
            foreach ( var product in data ) {
                _dbContext.AddProduct( product );
            }

            _dbContext.SaveChangesAsync();
        }
    }
}
