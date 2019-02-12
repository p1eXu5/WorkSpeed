using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
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
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts
{
    public class ImportService : Service, IImportService
    {
        private readonly ITypeRepository _typeRepository;

        private readonly object _locker = new Object();

        private Employee[] _employees;
        private Address[] _addresses;

        private readonly HashSet< Employee > _newEmployees = new HashSet< Employee >( ComparerFactory.EmployeeComparer );
        private readonly HashSet< Address > _newAddresses = new HashSet< Address >( ComparerFactory.AddressComparer );
        private readonly HashSet< Product > _newProducts = new HashSet< Product >( ComparerFactory.ProductComparer );


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


        private async void StoreData ( IEnumerable< Product > data )
        {
            var newProducts = new List< Product >();
            var dbProductList = await _dbContext.GetProducts().ToListAsync();

            foreach ( var product in data.Where( Check.IsProductCorrect ) ) {

                var dbProduct = dbProductList.FirstOrDefault( p => p.Id == product.Id );
                var newProduct = newProducts.FirstOrDefault( p => p.Id == product.Id );

                if ( dbProduct == null && newProduct == null ) {
                    newProducts.Add( product );
                    continue;
                }

                if ( dbProduct != null ) {
                    Check.CheckProductDifference( product, dbProduct );
                }
                else {
                    Check.CheckProductDifference( product, newProduct ); 
                }
            }

            await _dbContext.AddRangeAsync( newProducts );
            await _dbContext.SaveChangesAsync();
        }

        private async void StoreData ( IEnumerable< Employee > data )
        {
            var newEmployees = new List< Employee >();

            var dbEmployees = await _dbContext.GetEmployees().ToArrayAsync();
            var dbAppointments = await _dbContext.GetAppointments().ToArrayAsync();
            var dbRanks = await _dbContext.GetRanks().ToArrayAsync();
            var dbPositions = await _dbContext.GetPositions().ToArrayAsync();

            foreach ( var employee in data.Where( Check.IsEmployeeCorrect ) ) {

                employee.Rank = dbRanks.FirstOrDefault( r => r.Number == employee.Rank?.Number );
                employee.Appointment = dbAppointments.FirstOrDefault( a => Check.CheckAbbreviation( a.Abbreviations, employee.Appointment?.Abbreviations ) );
                employee.Position = dbPositions.FirstOrDefault( p => Check.CheckAbbreviation( p.Abbreviations, employee.Position?.Abbreviations ) );

                var dbEmployee = dbEmployees.FirstOrDefault( e => e.Id.Equals( employee.Id ) );
                var newEmployee = newEmployees.FirstOrDefault( e => e.Id.Equals( employee.Id ) );
                
                if ( dbEmployee == null && newEmployee == null  ) {
                    
                    newEmployees.Add( employee );
                    continue;
                }

                if ( dbEmployee != null ) {
                    Check.CheckEmployeeDifference( employee, dbEmployee );
                }
                else {
                    Check.CheckEmployeeDifference( employee, newEmployee );
                }
            }

            await _dbContext.AddRangeAsync( newEmployees );
            await _dbContext.SaveChangesAsync();
        }

        

        [ SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" ) ]
        private void StoreData ( IEnumerable< AllActions > data )
        {
            var actions = data.ToArray();

            var doubleAddressActions = actions.Select( a => a.DoubleAddressAction ).ToArray();
            var receptionActions = actions.Select( a => a.ReceptionAction ).ToArray();
            var inventoryActions = actions.Select( a => a.InventoryAction ).ToArray();
            var shipmentActions = actions.Select( a => a.ShipmentAction ).ToArray();
            var otherActions = actions.Select( a => a.OtherAction ).ToArray();

            lock ( _locker ) {
                
                _employees = _dbContext.GetEmployees().ToArray();
                _addresses = _dbContext.GetAddresses().ToArray();

                _newEmployees.Clear();
                _newAddresses.Clear();
                _newProducts.Clear();

                Check._nowDate = DateTime.Now;

                if ( doubleAddressActions[0] != null ) {
                    StoreDoubleActions( doubleAddressActions );
                }

                if ( receptionActions[0] != null ) {
                    StoreReceptionActions( receptionActions );
                }

                if ( inventoryActions[0] != null ) {
                    StoreInventoryActions( inventoryActions );
                }

                if ( shipmentActions[0] != null ) {
                    StoreShipmentActions( shipmentActions );
                }

                if ( otherActions[0] != null ) {
                    StoreOtherActions( otherActions );
                }

                _dbContext.SaveChanges();
            }
        }

        private async void StoreDoubleActions ( DoubleAddressAction[] data )
        {

            var actions = data.AsParallel()
                              .Where( a => Check.IsEmployeeBaseActionCorrect( a ) 
                                           && Check.IsProductCorrect(a.DoubleAddressDetails[0].Product)
                                           && Check.IsAddressCorrect(a.DoubleAddressDetails[0].SenderAddress)
                                           && Check.IsAddressCorrect(a.DoubleAddressDetails[0].ReceiverAddress) )
                              .AsSequential()
                              .GroupBy( a => a.Id ).ToArray();

            var newActions = new HashSet< DoubleAddressAction >( ComparerFactory.EmployeeActionBaseComparer );

            var periodStart = actions.Min( a => a.First().StartTime );
            var periodEnd = actions.Max( a => a.First().StartTime );
            if ( periodStart == periodEnd ) periodEnd += TimeSpan.FromMilliseconds( 1 );

            var dbActions = await _dbContext.GetDoubleAddressActions( periodStart, periodEnd ).ToArrayAsync();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Gathering ).ToArray();

            foreach ( var actionGrouping in actions ) {

                // check action
                var action = actionGrouping.First();

                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithEmployeeAction( action );

                if ( !actionGrouping.All( a => a.Employee.Id.Equals( action.Employee.Id ) && a.Operation.Name.Equals( action.Operation.Name ) )) continue;

                var details = new List< DoubleAddressActionDetail >();

                foreach ( var detail in actionGrouping.Select( a => a.DoubleAddressDetails[0] ) ) {
                    
                    // check product
                    await CheckProduct( detail );

                    // check addresses
                    CheckAddress( detail.ReceiverAddress, a => detail.ReceiverAddress = a );
                    CheckAddress( detail.SenderAddress, a => detail.SenderAddress = a );

                    detail.DoubleAddressAction = action;
                    detail.DoubleAddressActionId = action.Id;

                    details.Add( detail );
                }

                action.DoubleAddressDetails = details;
                newActions.Add( action );
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreReceptionActions ( ReceptionAction[] data )
        {
            var actions = data.AsParallel()
                              .Where( a => Check.IsEmployeeBaseActionCorrect( a ) 
                                           && Check.IsProductCorrect( a.ReceptionActionDetails[0].Product )
                                           && Check.IsAddressCorrect( a.ReceptionActionDetails[0].Address ) )
                              .AsSequential()
                              .GroupBy( a => a.Id ).ToArray();

            var newActions = new HashSet< ReceptionAction >( ComparerFactory.EmployeeActionBaseComparer );

            var periodStart = actions.Min( a => a.First().StartTime );
            var periodEnd = actions.Max( a => a.First().StartTime );
            if ( periodStart == periodEnd ) periodEnd += TimeSpan.FromMilliseconds( 1 );

            var dbActions = await _dbContext.GetReceptionActions( periodStart, periodEnd ).ToArrayAsync();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Reception ).ToArray();

            foreach ( var actionGrouping in actions ) {

                // check action
                var action = actionGrouping.First();
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithEmployeeAction( action );

                if ( !actionGrouping.All( a => a.Employee.Id.Equals( action.Employee.Id ) && a.Operation.Name.Equals( action.Operation.Name ) )) continue;

                var details = new List< ReceptionActionDetail >();

                foreach ( var detail in actionGrouping.Select( a => a.ReceptionActionDetails[0] ) ) {
                    
                    // check product
                    await CheckProduct( detail );

                    // check addresses
                    CheckAddress( detail.Address, a => detail.Address = a );

                    detail.ReceptionAction = action;
                    detail.ReceptionActionId = action.Id;

                    details.Add( detail );
                }

                action.ReceptionActionDetails = details;
                newActions.Add( action );
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreInventoryActions ( InventoryAction[] data  )
        {
            var actions = data.AsParallel()
                              .Where( a => Check.IsEmployeeBaseActionCorrect( a ) 
                                           && Check.IsProductCorrect( a.InventoryActionDetails[0].Product )
                                           && Check.IsAddressCorrect( a.InventoryActionDetails[0].Address ) )
                              .AsSequential()
                              .GroupBy( a => a.Id ).ToArray();

            var newActions = new HashSet<InventoryAction>(ComparerFactory.EmployeeActionBaseComparer);

            var periodStart = actions.Min( a => a.First().StartTime );
            var periodEnd = actions.Max( a => a.First().StartTime );
            if ( periodStart == periodEnd ) periodEnd += TimeSpan.FromMilliseconds( 1 );

            var dbActions = await _dbContext.GetInventoryActions( periodStart, periodEnd ).ToArrayAsync();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Inventory ).ToArray();

            foreach ( var actionGrouping in actions ) {

                // check action
                var action = actionGrouping.First();
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithEmployeeAction( action );

                if ( !actionGrouping.All( a => a.Employee.Id.Equals( action.Employee.Id ) && a.Operation.Name.Equals( action.Operation.Name ) )) continue;

                var details = new List< InventoryActionDetail >();

                foreach ( var detail in actionGrouping.Select( a => a.InventoryActionDetails[0] ) ) {
                    
                    // check product
                    await CheckProduct( detail );

                    // check addresses
                    CheckAddress( detail.Address, a => detail.Address = a );

                    detail.InventoryAction = action;
                    detail.InventoryActionId = action.Id;

                    details.Add( detail );
                }

                action.InventoryActionDetails = details;
                newActions.Add( action );
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreShipmentActions ( ShipmentAction[] data )
        {
            var newActions = new HashSet< ShipmentAction >( ComparerFactory.EmployeeActionBaseComparer );

            var actions = data.Where( Check.IsEmployeeBaseActionCorrect ).ToArray();

            var periodStart = actions.Min( a => a.StartTime );
            var periodEnd = actions.Max( a => a.StartTime );
            if ( periodStart == periodEnd ) periodEnd += TimeSpan.FromMilliseconds( 1 );

            var dbActions = await _dbContext.GetShipmentActions( periodStart, periodEnd ).ToArrayAsync();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Shipment ).ToArray();

            foreach ( var action in actions ) {
                // check action
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) && a.EmployeeId.Equals( action.EmployeeId ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithEmployeeAction( action );

                newActions.Add( action );
            }

            await _dbContext.AddRangeAsync( newActions );
        }

        private async void StoreOtherActions ( OtherAction[] data )
        {
            var newActions = new HashSet< OtherAction >( ComparerFactory.EmployeeActionBaseComparer );

            var actions = data.Where( Check.IsEmployeeBaseActionCorrect ).ToArray();

            var periodStart = actions.Min( a => a.StartTime );
            var periodEnd = actions.Max( a => a.StartTime );
            if ( periodStart == periodEnd ) periodEnd += TimeSpan.FromMilliseconds( 1 );

            var dbActions = await _dbContext.GetOtherActions( periodStart, periodEnd ).ToArrayAsync();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Other ).ToArray();

            foreach ( var action in actions ) {
                // check action
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithEmployeeAction( action );

                newActions.Add( action );
            }

            await _dbContext.AddRangeAsync( newActions );
        }


        private void CheckWithEmployeeAction< T > ( T action) where T : EmployeeActionBase
        {
            var dbEmployee = _employees.FirstOrDefault( e => e.Id.Equals( action.Employee.Id ) );
            var newEmployee = _newEmployees.FirstOrDefault( e => e.Id.Equals( action.Employee.Id ) );

            if ( dbEmployee == null && newEmployee == null ) {
                _newEmployees.Add( action.Employee );
            }
            else if ( dbEmployee != null ) {
                action.Employee = dbEmployee;
            }
            else {
                action.Employee = newEmployee;
            }
        }

        private async Task CheckProduct ( WithProductActionDetail detail )
        {
            var dbProduct = await _dbContext.GetProductAsync( detail.Product );
            var newProduct = _newProducts.FirstOrDefault( p => p.Id == detail.Product.Id );

            if ( dbProduct?.Parent == null && newProduct == null ) {
                if ( detail.Product.Parent != null ) {
                    if ( !Check.IsProductCorrect( detail.Product.Parent ) ) {
                        detail.Product.Parent = null;
                    }
                    else if ( detail.Product.Parent.Parent != null && !Check.IsProductCorrect( detail.Product.Parent.Parent ) ) {
                        detail.Product.Parent.Parent = null;
                    }
                }

                if ( dbProduct != null ) {
                    dbProduct.Parent = detail.Product.Parent;
                }
                else {
                    _newProducts.Add( detail.Product );
                }
            }

            if ( dbProduct != null ) {
                detail.Product = dbProduct;
                detail.ProductId = dbProduct.Id;
            }
            else if ( newProduct != null ) {
                detail.Product = newProduct;
                detail.ProductId = newProduct.Id;
            }
        }

        private void CheckAddress ( Address address, Action< Address > onChange )
        {
            var dbAddress = _addresses.FirstOrDefault( adr => adr.Letter.Equals( address.Letter ) && adr.Row == address.Row
                                                                && adr.Section == address.Section && adr.Shelf == address.Shelf
                                                                && adr.Box == address.Box );

            if ( dbAddress != null ) {
                onChange?.Invoke( dbAddress );
                return;
            }

            var newAddress = _newAddresses.FirstOrDefault( adr => adr.Letter.Equals( address.Letter ) && adr.Row == address.Row
                                                                && adr.Section == address.Section && adr.Shelf == address.Shelf
                                                                && adr.Box == address.Box );

            if ( newAddress != null ) {
                onChange?.Invoke( newAddress );
                return;
            }

            _newAddresses.Add( address );
            return;
        }


        private static class Check
        {
            internal static DateTime _nowDate;
            private static readonly DateTime DNS_BEGIN_TIME = new DateTime( 1998, 1, 1, 0, 0, 0);

            public static bool IsEmployeeBaseActionCorrect ( EmployeeActionBase a )
            {
                if ( a.Id.Length < 10 ) return false;
                int.TryParse( a.Id.Substring( 4 ), out var num );

                return num > 0
                       && !string.IsNullOrWhiteSpace( a.Id )
                       && char.IsLetter( a.Id[ 0 ] ) && char.IsLetter( a.Id[ 1 ] )
                       && char.IsDigit( a.Id[ 2 ] ) && a.Id[ 3 ].Equals( '-' )
                       && a.StartTime >= DNS_BEGIN_TIME && a.StartTime < _nowDate && a.Duration > TimeSpan.Zero
                       && IsEmployeeCorrect( a.Employee )
                       && a.Operation?.Name?.Length > 1;
            }

            public static bool IsProductCorrect ( Product p )
            {
                return p != null && !String.IsNullOrWhiteSpace( p.Name ) && p.Id > 0;
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

            public static bool IsAddressCorrect ( Address a )
            {
                return a != null && char.IsLetter( a.Letter[ 0 ] ) && a.Row > 0 && a.Section > 0 && a.Shelf > 0 && a.Box > 0;
            }


            public static bool CheckAbbreviation ( string abbreviations, string abbreviation )
            {
                var abreviations = abbreviations.Split( new[] { ';' } );
                return abreviations.Contains( abbreviation );
            }

            public static void CheckProductDifference ( Product donor, Product acceptor )
            {
                acceptor.ItemLength = TryUpdateMeasure( donor.ItemLength, acceptor.ItemLength );
                acceptor.ItemWidth = TryUpdateMeasure( donor.ItemWidth, acceptor.ItemWidth );
                acceptor.ItemHeight = TryUpdateMeasure( donor.ItemHeight, acceptor.ItemHeight );

                acceptor.ItemWeight = TryUpdateMeasure( donor.ItemWeight, acceptor.ItemWeight );

                acceptor.CartonLength = TryUpdateMeasure( donor.CartonLength, acceptor.CartonLength );
                acceptor.CartonWidth = TryUpdateMeasure( donor.CartonWidth, acceptor.CartonWidth );
                acceptor.CartonHeight = TryUpdateMeasure( donor.CartonHeight, acceptor.CartonHeight );

                acceptor.CartonQuantity = TryUpdateQuantity( donor.CartonQuantity, acceptor.CartonQuantity );


                float? TryUpdateMeasure ( float? donorValue, float? acceptorValue )
                {
                    if ( !acceptorValue.HasValue && donorValue.HasValue ) {
                        acceptorValue = donorValue;
                    }

                    return acceptorValue;
                }

                int? TryUpdateQuantity ( int? donorValue, int? acceptorValue )
                {
                    if ( !acceptorValue.HasValue && donorValue.HasValue ) {
                        acceptorValue = donorValue;
                    }

                    return acceptorValue;
                }
            }

            public static void CheckEmployeeDifference ( Employee donor, Employee acceptor )
            {
                if ( acceptor.Rank == null && donor.Rank != null ) {
                    acceptor.Rank = donor.Rank;
                }

                if ( acceptor.Appointment == null && donor.Appointment != null ) {
                    acceptor.Appointment = donor.Appointment;
                }

                if ( acceptor.Position == null && donor.Position != null ) {
                    acceptor.Position = donor.Position;
                }
            }
        }
    }
}
