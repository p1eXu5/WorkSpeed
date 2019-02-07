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
            return Task.Run( () => ImportFromXlsx( fileName, progress ), cancellationToken );
        }

        public void ImportFromXlsx ( string fileName, IProgress< (int, string) > progress )
        {
            try {
                var data = GetDataFromFile( fileName );
           
                if ( data.Any() ) {
                    StoreData( ( dynamic )data );
                }
            }
            catch ( Exception ) {
                progress?.Report( (-1, @"Не удалось прочитать файл. Файл либо открыт в другой программе, " 
                                          + @"либо содержит таблицу, тип которой определить не удалось.") );
            }
        }

        protected internal virtual IEnumerable< IEntity > GetDataFromFile ( string fileName )
        {
            var table = ExcelImporter.GetSheetTable( fileName );
            var typeMap = _typeRepository.GetTypeAndPropertyMap( table );
            return ExcelImporter.GetDataFromTable( table, typeMap.propertyMap, new ImportModelConverter() );
        }


        [ SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" ) ]
        private async void StoreData ( IEnumerable< Product > data )
        {
            var newProducts = new List< Product >( data.Count() );
            var updateProducts = new List< Product >( data.Count() );

            foreach ( var product in data.Where( p => !String.IsNullOrWhiteSpace( p.Name ) && p.Id > 0 ) ) {

                var dbProduct = await _dbContext.GetProductAsync( product );

                if ( dbProduct == null ) {
                    newProducts.Add( product );
                    continue;
                }

                if ( CheckDifferent( product, dbProduct ) ) {
                    updateProducts.Add( dbProduct );
                }
            }

            var addTask = _dbContext.AddRangeAsync( newProducts );
            _dbContext.UpdateRange( updateProducts );

            addTask.Wait();
            await _dbContext.SaveChangesAsync();
        }

        private bool CheckDifferent ( Product donor, Product acceptor )
        {
            bool isDifferent = false;

            acceptor.ItemLength = TryUpdateMeasure( donor.ItemLength, acceptor.ItemLength );
            acceptor.ItemWidth = TryUpdateMeasure( donor.ItemWidth, acceptor.ItemWidth );
            acceptor.ItemHeight = TryUpdateMeasure( donor.ItemHeight, acceptor.ItemHeight );

            acceptor.ItemWeight = TryUpdateMeasure( donor.ItemWeight, acceptor.ItemWeight );

            acceptor.CartonLength = TryUpdateMeasure( donor.CartonLength, acceptor.CartonLength );
            acceptor.CartonWidth = TryUpdateMeasure( donor.CartonWidth, acceptor.CartonWidth );
            acceptor.CartonHeight = TryUpdateMeasure( donor.CartonHeight, acceptor.CartonHeight );

            acceptor.CartonQuantity = TryUpdateQuantity( donor.CartonQuantity, acceptor.CartonQuantity );

            return isDifferent;

            float? TryUpdateMeasure ( float? donorValue, float? acceptorValue )
            {
                if ( !acceptorValue.HasValue && donorValue.HasValue ) {
                    acceptorValue = donorValue;
                    if ( !isDifferent ) isDifferent = true;
                }

                return acceptorValue;
            }

            int? TryUpdateQuantity ( int? donorValue, int? acceptorValue )
            {
                if ( !acceptorValue.HasValue && donorValue.HasValue ) {
                    acceptorValue = donorValue;
                    if ( !isDifferent ) isDifferent = true;
                }

                return acceptorValue;
            }
        }

        private static class Check
        {
        }
    }
}
