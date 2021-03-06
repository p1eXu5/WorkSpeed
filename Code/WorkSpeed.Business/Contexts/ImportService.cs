﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Agbm.Helpers.Extensions;
using Agbm.NpoiExcel;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Business.Contexts.Comparers;
using WorkSpeed.Business.FileModels;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Context.ImportService;


namespace WorkSpeed.Business.Contexts
{
    public class ImportService : Service, IImportService
    {
        #region Fields

        private readonly ITypeRepository _typeRepository;

        private IProgress< (int, string) > _progress;
        private readonly object _locker = new Object();

        private Employee[] _employees;
        private Address[] _addresses;
        private Product[] _products;

        private readonly HashSet< Employee > _newEmployees = new HashSet< Employee >( ComparerFactory.EmployeeComparer );
        private readonly HashSet< Address > _newAddresses = new HashSet< Address >( ComparerFactory.AddressComparer );
        private readonly HashSet< Product > _newProducts = new HashSet< Product >( ComparerFactory.ProductComparer );

        private Position _defaultPosition;
        private Appointment _defaultAppointment;
        private Rank _defaultRank;
        private Shift _defaultShift;
        private ShortBreakSchedule _defaultBreaks;
        private Avatar _defaultAvatar;

        #endregion


        #region Ctor

        public ImportService ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext )
        {
            _typeRepository = typeRepository;
        }

        #endregion


        #region Properties

        private Position DefaultPosition => _defaultPosition ?? ( _defaultPosition = _dbContext.GetDefaultPosition() );
        private Appointment DefaultAppointment => _defaultAppointment ?? ( _defaultAppointment = _dbContext.GetDefaultAppointment() );
        private Rank DefaultRank => _defaultRank ?? _defaultRank ?? ( _defaultRank = _dbContext.GetDefaultRank() );
        private Shift DefaultShift => _defaultShift ?? ( _defaultShift = _dbContext.GetDefaultShift() );
        private ShortBreakSchedule DefaultBreaks => _defaultBreaks ?? ( _defaultBreaks = _dbContext.GetDefaultShortBreakSchedule() );
        private Avatar DefaultAvatar => _defaultAvatar ?? ( _defaultAvatar = _dbContext.GetDefaultAvatar() );

        #endregion


        public async Task ImportFromXlsxAsync ( string fileName, IProgress< (int, string) > progress, CancellationToken cancellationToken )
        {
            await Task.Run( () => ImportFromXlsx( fileName, progress ), cancellationToken );
        }

        public void ImportFromXlsx ( string fileName, IProgress< (int, string) > progress = null )
        {
            _progress = progress;

            try {
                _progress?.Report( (1, @"Чтение файла") );
                var data = GetDataFromFile( fileName );

                if ( data != null && data.Any() ) {
                    _progress?.Report( (50, @"Распознавание данных") );
                    StoreData( ( dynamic )data );
                }
            }
            catch ( FileNotFoundException ) {
                _progress?.Report( (-1, @"Файл не найден.") );
            }
            catch ( DirectoryNotFoundException ) {
                _progress?.Report( (-1, @"Путь к файлу не найден.") );
            }
            catch ( DbUpdateException ex ) {
                _progress?.Report( (-1, @"Не удалось сохранить експортируемые данные." + $"\n{ex.Message}") );
            }
            catch ( Exception ex ) {
                _progress?.Report( (-1, @"Не удалось прочитать файл. Файл либо открыт в другой программе, " 
                                          + @"либо содержит таблицу, тип которой определить не удалось." + $"\n{ex.Message}") );
            }
        }


        protected internal virtual IEnumerable< IEntity > GetDataFromFile ( string fileName )
        {
            var table = ExcelImporter.GetSheetTable( fileName );
            var typeMap = _typeRepository.GetTypeAndPropertyMap( table );

            if ( typeMap.type == typeof( ProductImportModel ) ) {
                _progress?.Report( (10, @"Чтение из файла данных о товаре") );
                return ExcelImporter.GetDataFromTable( table, typeMap.propertyMap, new ImportModelConverter< ProductImportModel, Product >() );
            }

            if ( typeMap.type == typeof( EmployeeImportModel ) ) {
                _progress?.Report( (10, @"Чтение из файла данных о сотрудниках") );
                return ExcelImporter.GetDataFromTable( table, typeMap.propertyMap, new ImportModelConverter< EmployeeImportModel, Employee >() );
            }

            if ( typeMap.type == typeof( EmployeeShortImportModel ) ) {
                _progress?.Report( (10, @"Чтение из файла данных о сотрудниках") );
                return ExcelImporter.GetDataFromTable( table, typeMap.propertyMap, new ImportModelConverter< EmployeeShortImportModel, Employee >() );
            }

            if ( typeMap.type == typeof( ProductivityImportModel ) ) {
                _progress?.Report( (10, @"Чтение из файла данных о действиях сотрудников") );
                return ExcelImporter.GetDataFromTable( table, typeMap.propertyMap, new ImportModelConverter< ProductivityImportModel, EmployeeActionBase >() );
            }


            return ( IEnumerable< IEntity > )null;
        }


        private void StoreData ( IEnumerable< Product > data )
        {
            var newProducts = new List< Product >();
            var dbProductList = _dbContext.GetProducts().ToList();

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

            _progress?.Report( (90, @"Сохрание данных") );
            _dbContext.AddRange( newProducts );
            _dbContext.SaveChanges();
        }

        private void StoreData ( IEnumerable< Employee > data )
        {
            var newEmployees = new List< Employee >();

            var dbEmployees = _dbContext.GetEmployees().ToArray();
            var dbAppointments = _dbContext.GetAppointments().ToArray();
            var dbRanks = _dbContext.GetRanks().ToArray();
            var dbPositions = _dbContext.GetPositions().ToArray();

            foreach ( var employee in data.Where( Check.IsEmployeeCorrect ) ) {

                employee.Rank = dbRanks.FirstOrDefault( r => r.Number == employee.Rank?.Number ) ?? DefaultRank;

                employee.Appointment = employee.Appointment?.Abbreviations == null 
                                           ? DefaultAppointment 
                                           : dbAppointments.FirstOrDefault( a => Check.CheckAbbreviation( a.Abbreviations, employee.Appointment.Abbreviations ) ) ?? DefaultAppointment;

                employee.Position = employee.Position?.Abbreviations == null 
                                        ? DefaultPosition 
                                        : dbPositions.FirstOrDefault( p => Check.CheckAbbreviation( p.Abbreviations, employee.Position?.Abbreviations ) ) ?? DefaultPosition;

                employee.Shift = DefaultShift;
                employee.ShortBreakSchedule = DefaultBreaks;
                employee.Avatar = DefaultAvatar;

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

            _progress?.Report( (90, @"Сохрание данных") );
            _dbContext.AddRange( newEmployees );
            _dbContext.SaveChanges();
        }

        

        [ SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" ) ]
        private void StoreData ( IEnumerable< EmployeeActionBase > data )
        {
            var doubleAddressActions = new List< DoubleAddressAction >();
            var receptionActions = new List< ReceptionAction >();
            var inventoryActions = new List< InventoryAction >();
            var shipmentActions = new List< ShipmentAction >();
            var otherActions = new List< OtherAction >();

            foreach ( var action in data ) {

                switch ( action ) {
                    case DoubleAddressAction doubleAddressAction :
                        doubleAddressActions.Add( doubleAddressAction );
                        continue;
                    case ReceptionAction receptionAction :
                        receptionActions.Add( receptionAction );
                        continue;
                    case InventoryAction inventoryAction :
                        inventoryActions.Add( inventoryAction );
                        continue;
                    case ShipmentAction shipmentAction :
                        shipmentActions.Add( shipmentAction );
                        continue;
                    case OtherAction otherAction :
                        otherActions.Add( otherAction );
                        break;
                }
            }

            lock ( _locker ) {
                
                _employees = _dbContext.GetEmployees().ToArray();
                _addresses = _dbContext.GetAddresses().ToArray();
                _products = _dbContext.GetProducts().ToArray();

                _newEmployees.Clear();
                _newAddresses.Clear();
                _newProducts.Clear();

                Check._nowDate = DateTime.Now;

                if ( doubleAddressActions.Any() ) {
                    _progress?.Report( (51, @"Распознавание данных. Отгрузка.") );
                    StoreDoubleActions( doubleAddressActions );
                }

                if ( receptionActions.Any() ) {
                    _progress?.Report( (61, @"Распознавание данных. Приёмка.") );
                    StoreReceptionActions( receptionActions );
                }

                if ( inventoryActions.Any() ) {
                    _progress?.Report( (71, @"Распознавание данных. Инвентаризация.") );
                    StoreInventoryActions( inventoryActions );
                }

                if ( shipmentActions.Any() ) {
                    _progress?.Report( (81, @"Распознавание данных. ПРР.") );
                    StoreShipmentActions( shipmentActions );
                }

                if ( otherActions.Any() ) {
                    _progress?.Report( (86, @"Распознавание данных. Остальные операции.") );
                    StoreOtherActions( otherActions );
                }

                _progress?.Report( (90, @"Сохрание данных") );
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        ///     Prepares gathering actions for saving in db if they are.
        /// </summary>
        /// <param name="data"></param>
        private void StoreDoubleActions ( IEnumerable< DoubleAddressAction > data )
        {
            var actions = data.AsParallel()
                              .Where( a => Check.IsEmployeeBaseActionCorrect( a ) 
                                           && Check.IsProductCorrect(a.DoubleAddressDetails[0].Product) )
                              .AsSequential()
                              .GroupBy( a => a.Id ).ToArray();

            var newActions = new HashSet< DoubleAddressAction >( ComparerFactory.EmployeeActionBaseComparer );

            var periodStart = actions.Min( a => a.First().StartTime );
            var periodEnd = actions.Max( a => a.First().StartTime );

            var dbActions = _dbContext.GetDoubleAddressActions( periodStart, periodEnd ).ToArray();
            var dbOperations = _dbContext.GetOperations( new HashSet< OperationGroups > {
                OperationGroups.BuyerGathering,
                OperationGroups.Defragmentation,
                OperationGroups.Gathering,
                OperationGroups.Packing,
                OperationGroups.Placing
            }).ToArray();

            foreach ( var actionGrouping in actions ) {

                // check action
                var action = actionGrouping.First();

                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithExistedAction( action );

                if ( !actionGrouping.All( a => a.Employee.Id.Equals( action.Employee.Id ) && a.Operation.Name.Equals( action.Operation.Name ) )) continue;

                var details = new List< DoubleAddressActionDetail >();

                foreach ( var detail in actionGrouping.Select( a => a.DoubleAddressDetails[0] ) ) {
                    
                    // check product
                    CheckProduct( detail );

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

            _dbContext.AddRange( newActions );
        }

        /// <summary>
        ///     Prepares reception actions for saving in db if they are.
        /// </summary>
        /// <param name="data"></param>
        private void StoreReceptionActions ( IEnumerable< ReceptionAction > data )
        {
            var actions = data.AsParallel()
                              .Where( a => Check.IsEmployeeBaseActionCorrect( a ) 
                                           && Check.IsProductCorrect( a.ReceptionActionDetails[0].Product ) )
                              .AsSequential()
                              .GroupBy( a => a.Id ).ToArray();

            var newActions = new HashSet< ReceptionAction >( ComparerFactory.EmployeeActionBaseComparer );

            var periodStart = actions.Min( a => a.First().StartTime );
            var periodEnd = actions.Max( a => a.First().StartTime );
            var dbActions = _dbContext.GetReceptionActions( periodStart, periodEnd ).ToArray();

            var dbOperations = _dbContext.GetOperations( OperationGroups.Reception ).ToArray();
            Operation clientOperation = null;
            Operation nonClientOperation = null;
            if ( dbOperations[ 0 ].Name.Contains( "транзитов" ) ) {
                clientOperation = dbOperations[ 0 ];
                nonClientOperation = dbOperations[ 1 ];
            }
            else {
                clientOperation = dbOperations[ 1 ];
                nonClientOperation = dbOperations[ 0 ];
            }

            foreach ( var actionGrouping in actions ) {

                // check action
                var firstActionInGroup = actionGrouping.First();
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( firstActionInGroup.Id ) );
                if ( dbAction != null ) continue;
                if ( !actionGrouping.All( a => a.Employee.Id.Equals( firstActionInGroup.Employee.Id ) && a.Operation.Name.Equals( firstActionInGroup.Operation.Name ) )) continue;

                foreach ( var act in actionGrouping ) {
                    
                    CheckWithExistedAction( act );
                    firstActionInGroup.Operation = firstActionInGroup.ReceptionActionDetails[0].IsClientScanning 
                                       ? clientOperation
                                       : nonClientOperation;
                }


                var details = new List< ReceptionActionDetail >();

                foreach ( var detail in actionGrouping.Select( a => a.ReceptionActionDetails[0] ) ) {
                    
                    // check product
                    CheckProduct( detail );

                    // check addresses
                    CheckAddress( detail.Address, a => detail.Address = a );

                    detail.ReceptionAction = firstActionInGroup;
                    detail.ReceptionActionId = firstActionInGroup.Id;

                    details.Add( detail );
                }

                firstActionInGroup.ReceptionActionDetails = details;
                newActions.Add( firstActionInGroup );
            }

            _dbContext.AddRange( newActions );
        }

        /// <summary>
        ///     Prepares inventory actions for saving in db if they are.
        /// </summary>
        /// <param name="data"></param>
        private void StoreInventoryActions ( IEnumerable< InventoryAction > data  )
        {
            var actions = data.AsParallel()
                              .Where( a => Check.IsEmployeeBaseActionCorrect( a ) 
                                           && Check.IsProductCorrect( a.InventoryActionDetails[0].Product ) )
                              .AsSequential()
                              .GroupBy( a => a.Id ).ToArray();

            var newActions = new HashSet<InventoryAction>(ComparerFactory.EmployeeActionBaseComparer);

            var periodStart = actions.Min( a => a.First().StartTime );
            var periodEnd = actions.Max( a => a.First().StartTime );
            var dbActions = _dbContext.GetInventoryActions( periodStart, periodEnd ).ToArray();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Inventory ).ToArray();

            foreach ( var actionGrouping in actions ) {

                // check action
                var action = actionGrouping.First();
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithExistedAction( action );

                if ( !actionGrouping.All( a => a.Employee.Id.Equals( action.Employee.Id ) && a.Operation.Name.Equals( action.Operation.Name ) )) continue;

                var details = new List< InventoryActionDetail >();

                foreach ( var detail in actionGrouping.Select( a => a.InventoryActionDetails[0] ) ) {
                    
                    // check product
                    CheckProduct( detail );

                    // check addresses
                    CheckAddress( detail.Address, a => detail.Address = a );

                    detail.InventoryAction = action;
                    detail.InventoryActionId = action.Id;

                    details.Add( detail );
                }

                action.InventoryActionDetails = details;
                newActions.Add( action );
            }

            _dbContext.AddRange( newActions );
        }

        /// <summary>
        ///     Prepares shipment actions for saving in db if they are.
        /// </summary>
        /// <param name="data"></param>
        private void StoreShipmentActions ( IEnumerable< ShipmentAction > data )
        {
            var newActions = new HashSet< ShipmentAction >( ComparerFactory.EmployeeActionBaseComparer );

            var actions = data.Where( Check.IsEmployeeBaseActionCorrect ).ToArray();

            var periodStart = actions.Min( a => a.StartTime );
            var periodEnd = actions.Max( a => a.StartTime );
            var dbActions = _dbContext.GetShipmentActions( periodStart, periodEnd ).ToArray();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Shipment ).ToArray();

            foreach ( var action in actions ) {
                // check action
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) && a.EmployeeId.Equals( action.EmployeeId ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithExistedAction( action );

                newActions.Add( action );
            }

            _dbContext.AddRange( newActions );
        }

        /// <summary>
        ///     Prepares other actions for saving in db if they are.
        /// </summary>
        /// <param name="data"></param>
        private void StoreOtherActions ( IEnumerable< OtherAction > data )
        {
            var newActions = new HashSet< OtherAction >( ComparerFactory.EmployeeActionBaseComparer );

            var actions = data.Where( Check.IsEmployeeBaseActionCorrect ).ToArray();

            var periodStart = actions.Min( a => a.StartTime );
            var periodEnd = actions.Max( a => a.StartTime );

            var dbActions = _dbContext.GetOtherActions( periodStart, periodEnd ).ToArray();
            var dbOperations = _dbContext.GetOperations( OperationGroups.Other ).ToArray();

            foreach ( var action in actions ) {
                // check action
                var dbAction = dbActions.FirstOrDefault( a => a.Id.Equals( action.Id ) );
                if ( dbAction != null ) continue;

                var dbOperation = dbOperations.FirstOrDefault( o => o.Name.Equals( action.Operation.Name ) );
                if ( null == dbOperation ) continue;
                action.Operation = dbOperation;

                CheckWithExistedAction( action );

                newActions.Add( action );
            }

            _dbContext.AddRange( newActions );
        }

        /// <summary>
        ///     Check does employee exist in db or has it alredy added in collection with new employees.
        ///     If hasn't then adds employee into collection with new employees.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        private void CheckWithExistedAction< T > ( T action) where T : EmployeeActionBase
        {
            var dbEmployee = _employees.FirstOrDefault( e => e.Id.Equals( action.Employee.Id ) );
            var newEmployee = _newEmployees.FirstOrDefault( e => e.Id.Equals( action.Employee.Id ) );

            if ( dbEmployee == null && newEmployee == null ) {

                action.Employee.Position = DefaultPosition;
                action.Employee.Appointment = DefaultAppointment;
                action.Employee.Rank = DefaultRank;
                action.Employee.Shift = DefaultShift;
                action.Employee.ShortBreakSchedule = DefaultBreaks;
                action.Employee.Avatar = DefaultAvatar;

                _newEmployees.Add( action.Employee );
            }
            else if ( dbEmployee != null ) {
                action.Employee = dbEmployee;
            }
            else {
                action.Employee = newEmployee;
            }
        }

        private void CheckProduct ( WithProductActionDetail detail )
        {
            var dbProduct = _products.FirstOrDefault( p => p.Id == detail.ProductId );
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

                var res = num > 0
                    && !string.IsNullOrWhiteSpace( a.Id )
                    && char.IsLetter( a.Id[ 0 ] ) && char.IsLetter( a.Id[ 1 ] )
                    && char.IsDigit( a.Id[ 2 ] ) && a.Id[ 3 ].Equals( '-' )
                    && a.StartTime >= DNS_BEGIN_TIME && a.StartTime < _nowDate
                    && IsEmployeeCorrect( a.Employee )
                    && a.Operation?.Name?.Length > 1;
#if DEBUG
                if ( !res ) {
                    Debug.WriteLine( $"num: {num}; a.Id[ 0 ]: {a.Id[ 0 ]}; a.Id[ 1 ]: {a.Id[ 1 ]}; a.Id[ 2 ]: {a.Id[ 2 ]}; a.Id[ 3 ]: {a.Id[ 3 ]};" );
                    Debug.WriteLine( $"a.StartTime: {a.StartTime}; DNS_BEGIN_TIME: {DNS_BEGIN_TIME};" );
                    Debug.WriteLine( $"IsEmployeeCorrect: {IsEmployeeCorrect( a.Employee )}; a.Operation?.Name?.Length: {a.Operation?.Name?.Length};" );
                }
#endif
                return res;
            }

            public static bool IsProductCorrect ( Product p )
            {
                var res = p != null && !String.IsNullOrWhiteSpace( p.Name ) && p.Id > 0;
#if DEBUG
                if ( !res ) {
                    Debug.WriteLine( $"p.Name: {p.Name}; p.Id: {p.Id};" );
                }
#endif
                return res;
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


            public static bool CheckAbbreviation ( string abbreviations, string abbreviation )
            {
                var abreviations = abbreviations.Split( new[] { ';' } );
                return abreviations.Contains( abbreviation.RemoveWhitespaces() );
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

            /// <summary>
            /// Check new or changed data.
            /// </summary>
            /// <param name="donor">Current employee</param>
            /// <param name="acceptor">Db or handled new employee</param>
            public static void CheckEmployeeDifference ( Employee donor, Employee acceptor )
            {
                if ( acceptor.Rank != donor.Rank ) {
                    acceptor.Rank = donor.Rank;
                }

                if ( acceptor.Appointment != donor.Appointment ) {
                    acceptor.Appointment = donor.Appointment;
                }

                if ( acceptor.Position != donor.Position ) {
                    acceptor.Position = donor.Position;
                }

                if ( acceptor.IsActive != donor.IsActive ) {
                    acceptor.IsActive = donor.IsActive;
                }
            }
        }
    }
}
