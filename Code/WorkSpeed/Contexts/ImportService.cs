using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
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


        #region Ctor

        public ImportService ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext )
        {
            _typeRepository = typeRepository;
        }

        #endregion


        public Task ImportFromXlsxAsync ( string fileName, IProgress< (int, string) > progress, CancellationToken cancellationToken )
        {
            var task = new Task( () => ImportFromXlsx( fileName, progress ), cancellationToken );
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

        [ SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" ) ]
        private async void StoreData ( IEnumerable< Product > data )
        {
            var products = new List< Product >( data.Count() );

            foreach ( var product in data ) {
                products.Add( product );
            }

            await _dbContext.AddRangeAsync( products );
            await _dbContext.SaveChangesAsync();
        }
    }
}
