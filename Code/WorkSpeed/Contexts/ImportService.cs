using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Business.Contexts.Comparers;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

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
            catch ( Exception ex ) {
                progress?.Report( (-1, @"Не удалось прочитать файл. Файл либо открыт в другой программе, " 
                                          + @"либо содержит таблицу, тип которой определить не удалось.") );

                throw;
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
            var products = new HashSet< Product >( data, ComparerFactory.ProductComparer );
            var newProducts = new List< Product >( data.Count() );
            var updateProducts = new List< Product >( data.Count() );
            var dbProductList = await _dbContext.GetProducts().ToListAsync();

            foreach ( var product in products.Where( Check.IsProductCorrect ) ) {

                var dbProduct = dbProductList.FirstOrDefault( p => p.Id == product.Id );

                if ( dbProduct == null ) {
                    newProducts.Add( product );
                    continue;
                }

                if ( Check.CheckDifferent( product, dbProduct ) ) {
                    updateProducts.Add( dbProduct );
                }
            }

            var addTask = _dbContext.AddRangeAsync( newProducts );
            _dbContext.UpdateRange( updateProducts );

            addTask.Wait();
            await _dbContext.SaveChangesAsync();
        }

        

        [ SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" ) ]
        private async void StoreData ( IEnumerable< Employee > data )
        {
            // new Appointment, Rank and Position can be

            var employees = new HashSet< Employee >( data, ComparerFactory.EmployeeComparer );
            var newEmployees = new List< Employee >( data.Count() );

            var dbEmployees = await _dbContext.GetEmployees().ToArrayAsync();
            var dbAppointments = await _dbContext.GetAppointments().ToArrayAsync();
            var dbRanks = await _dbContext.GetRanks().ToArrayAsync();
            var dbPositions = await _dbContext.GetPositions().ToArrayAsync();

            foreach ( var employee in employees.Where( Check.IsEmployeeCorrect ) ) {

                employee.Rank = dbRanks.FirstOrDefault( r => r.Number == employee.Rank?.Number );
                employee.Appointment = dbAppointments.FirstOrDefault( a => Check.CheckAbbreviation( a.Abbreviations, employee.Appointment?.Abbreviations ) );
                employee.Position = dbPositions.FirstOrDefault( p => Check.CheckAbbreviation( p.Abbreviations, employee.Position?.Abbreviations ) );

                var dbEmployee = dbEmployees.FirstOrDefault( e => e.Id.Equals( employee.Id ) );

                if ( null == dbEmployee ) {
                    newEmployees.Add( employee );
                    continue;
                }

                dbEmployee.Rank = ( Rank )Check.CheckEntity( dbEmployee.Rank, employee.Rank );
                dbEmployee.Appointment = ( Appointment )Check.CheckEntity( dbEmployee.Appointment, employee.Appointment );
                dbEmployee.Position = ( Position )Check.CheckEntity( dbEmployee.Position, employee.Position );
            }

            await _dbContext.AddRangeAsync( newEmployees );
            await _dbContext.SaveChangesAsync();

        }

        

        [ SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" ) ]
        private async void StoreData ( IEnumerable< AllActions > data )
        {
            var actions = data.ToArray();

            var doubleAddressActions = actions.Select( a => a.DoubleAddressAction ).ToArray();
            //var receptionActions = actions.Select( a => a.ReceptionAction ).ToArray();
            //var inventoryActions = actions.Select( a => a.InventoryAction ).ToArray();
            //var shipmentActions = actions.Select( a => a.ShipmentAction ).ToArray();
            //var otherActions = actions.Select( a => a.OtherAction ).ToArray();

            StoreDoubleActions( doubleAddressActions );
            //StoreReceptionActions( receptionActions );
            //StoreInventoryActions( inventoryActions );
            //StoreShipmentActions( shipmentActions );
            //StoreOtherActions( otherActions );

            await _dbContext.SaveChangesAsync();
        }

        private async void StoreOtherActions ( IEnumerable< OtherAction > data )
        {
            // new Operation and Employee can be
            var newActions = new List< OtherAction >();

            foreach ( var otherAction in data.Where( a => !string.IsNullOrWhiteSpace(a.Id ) && a.Id.Length == 11 ) ) {

                var dbAction = await _dbContext.GetOtherActionAsync( otherAction );

                if ( null == dbAction ) {
                    newActions.Add( otherAction );
                }
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreShipmentActions ( IEnumerable< ShipmentAction > data )
        {
            // new Operation and Employee can be
            var newActions = new List< ShipmentAction >();

            foreach ( var shipmentAction in data.Where( a => !string.IsNullOrWhiteSpace(a.Id ) && a.Id.Length == 11 ) ) {

                var dbAction = await _dbContext.GetShipmentActionAsync( shipmentAction );

                if ( null == dbAction ) {
                    newActions.Add( shipmentAction );
                }
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreInventoryActions ( IEnumerable< InventoryAction > data )
        {
            // new Operation, Employee, Product and ProductGroup can be
            var newActions = new List< InventoryAction >();

            foreach ( var inventoryAction in data.Where( a => !string.IsNullOrWhiteSpace(a.Id ) && a.Id.Length == 11 ) ) {

                var dbAction = await _dbContext.GetInventoryActionAsync( inventoryAction );

                if ( null == dbAction ) {
                    newActions.Add( inventoryAction );
                }
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreReceptionActions ( IEnumerable< ReceptionAction > data )
        {
            // new Operation, Employee, Product and ProductGroup can be
            var newActions = new List< ReceptionAction >();

            foreach ( var receptionAction in data.Where( a => !string.IsNullOrWhiteSpace(a.Id ) && a.Id.Length == 11 ) ) {

                var dbAction = await _dbContext.GetReceptionActionAsync( receptionAction );

                if ( null == dbAction ) {
                    newActions.Add( receptionAction );
                }
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreDoubleActions ( DoubleAddressAction[] data )
        {
            // new Operation, Employee, Product and ProductGroup can be
            var newActions = new List< DoubleAddressAction >();

                foreach ( var doubleAddressAction in data.Where( Check.IsDoubleActionCorrect ) ) {

                    Modify.RemoveAddresses( doubleAddressAction.DoubleAddressDetails[0] );

                    var dbAction = await _dbContext.GetDoubleAddressActionAsync( doubleAddressAction );

                    if ( null == dbAction ) {
                        newActions.Add( doubleAddressAction );
                    }
                }

                await _dbContext.AddRangeAsync( newActions );
        }



        private static class Check
        {
            public static bool IsDoubleActionCorrect ( DoubleAddressAction a )
            {

                return !string.IsNullOrWhiteSpace( a.Id ) && a.Id.Length == 10;
            }

            [ MethodImpl( MethodImplOptions.AggressiveInlining )]
            public static bool IsProductCorrect ( Product p )
            {
                return !String.IsNullOrWhiteSpace( p.Name ) && p.Id > 0;
            }

            public static bool IsEmployeeCorrect ( Employee e )
            {
                if ( e.Id.Length != 7 ) return false;

                int.TryParse( e.Id.Substring( 2 ), out var num );

                return num > 0
                       && !string.IsNullOrWhiteSpace( e.Name )
                       && !string.IsNullOrWhiteSpace( e.Id )
                       && e.Name.Length >= 1
                       && char.IsLetter( e.Id[0] ) && char.IsLetter( e.Id[1] );
            }

            [ MethodImpl( MethodImplOptions.AggressiveInlining )]
            public static bool CheckAbbreviation ( string abbreviations, string abbreviation )
            {
                var abreviations = abbreviations.Split( new[] { ';' } );
                return abreviations.Contains( abbreviation );
            }

            [ MethodImpl( MethodImplOptions.AggressiveInlining )]
            public static IEntity CheckEntity ( IEntity dbEntity, IEntity entity )
            {
                if ( dbEntity == null && entity != null ) {
                    dbEntity = entity;
                }

                return dbEntity;
            }

            public static bool CheckDifferent ( Product donor, Product acceptor )
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
        }

        private static class Modify
        {
            public static void RemoveAddresses ( DoubleAddressActionDetail detail )
            {
                if ( detail.SenderAddress != null )
                    detail.SenderAddress = null;

                 if ( detail.ReceiverAddress != null )
                    detail.ReceiverAddress = null;

            }

            public static void RemoveAddresses ( SingleAddressActionDetail detail )
            {
                if ( detail.Address != null )
                    detail.Address = null;
            }
        }
    }
}
