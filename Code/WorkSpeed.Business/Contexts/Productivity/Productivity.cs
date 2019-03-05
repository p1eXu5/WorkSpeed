﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Comparers;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.Data.Models.Extensions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    /// <summary>
    ///     Created for each operation.
    /// </summary>
    public class Productivity : IProductivity
    {
        private readonly Dictionary< EmployeeActionBase, Period > _actionPeriodMap;

        #region Ctor

        public Productivity ()
        {
            _actionPeriodMap = new Dictionary< EmployeeActionBase, Period >( new EmployeeActionBaseComparer< EmployeeActionBase >() );
        }

        #endregion


        #region Properties

        public IReadOnlyDictionary< EmployeeActionBase, Period > ActionPeriodMap => _actionPeriodMap;

        #endregion


        public void Add ( EmployeeActionBase action, Period period )
        {
            _actionPeriodMap[ action ] = period;
        }

        public Period this [ EmployeeActionBase action ]
        {
            get => _actionPeriodMap[ action ];
            set => _actionPeriodMap[ action ] = value;
        }

        public TimeSpan  GetTime ()
        {
            return _actionPeriodMap.Values.Aggregate( TimeSpan.Zero, (acc, next) => acc + next.Duration );
        }

        public double GetTotalHours ()
        {
            if ( _actionPeriodMap.Count == 0 ) { return 0.0; }

            return _actionPeriodMap.Values.Sum( p => p.Duration.TotalHours );
        }

        public double GetLinesPerHour ()
        {
            if ( _actionPeriodMap.Count == 0 ) { return 0.0; }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return 0.0; }

            double lines = 0.0;

            switch ( group ) {
                case OperationGroups.Undefined:
                    return 0.0;
                case OperationGroups.Reception:
                    lines = _actionPeriodMap.Keys.Cast< ReceptionAction >().Sum( r => r.ReceptionActionDetails.Count );
                    break;
                case OperationGroups.Inventory:
                    lines = _actionPeriodMap.Keys.Cast< InventoryAction >().Sum( r => r.InventoryActionDetails.Count );
                    break;
                default:
                    lines = _actionPeriodMap.Keys.Cast< DoubleAddressAction >().Sum( r => r.DoubleAddressDetails.Count );
                    break;
            }

            return lines / GetTotalHours();
        }

        public double GetScansPerHour ()
        {
            if ( _actionPeriodMap.Count == 0 ) { return 0.0; }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Reception ) { return 0.0; }

            var scans =  _actionPeriodMap.Keys.Cast< ReceptionAction >().Sum( r => r.ReceptionActionDetails.Sum( d => d.ScanQuantity ) );

            return scans / GetTotalHours();
        }

        public double GetTotalVolume ()
        {
            if ( _actionPeriodMap.Count == 0 ) { return 0.0; }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Shipment ) { return 0.0; }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return 0.0;
                case OperationGroups.Shipment:
                    return _actionPeriodMap.Keys.Cast< ShipmentAction >().Sum( r => r.Volume ?? 0.0 );
                case OperationGroups.Reception:
                    return _actionPeriodMap.Keys.Cast< ReceptionAction >().Sum( r => r.ReceptionActionDetails.Sum( d => d.Volume() ) );
                case OperationGroups.Inventory:
                    return _actionPeriodMap.Keys.Cast< InventoryAction >().Sum( r => r.InventoryActionDetails.Sum( d => d.Volume() )  );
                default:
                    return _actionPeriodMap.Keys.Cast< DoubleAddressAction >().Sum( r => r.DoubleAddressDetails.Sum( d => d.Volume() )  );
            }
        }

        public (double clientCargo, double nonClientCargo) GetCargoQuantity ()
        {
            if ( _actionPeriodMap.Count == 0 ) { return (0.0, 0.0); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Shipment ) { return (0.0, 0.0); }

            var clientCargo =  _actionPeriodMap.Keys.Cast< ShipmentAction >().Sum( r => Convert.ToDouble(r.ClientCargoQuantity) );
            var nonClientCargo =  _actionPeriodMap.Keys.Cast< ShipmentAction >().Sum( r =>  Convert.ToDouble(r.CommonCargoQuantity) );

            return (clientCargo, nonClientCargo);
        }

        public IEnumerable< (int count, Category category) > GetLines ( IEnumerable< Category > categories )
        {
            if ( _actionPeriodMap.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0, c));
                case OperationGroups.Reception:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< ReceptionAction >()
                                                             .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ) )
                                                             .Count(), c ) );
                case OperationGroups.Inventory:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< InventoryAction >()
                                                             .Select( a => a.InventoryActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ) )
                                                             .Count(), c ) );
                default:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< DoubleAddressAction >()
                                                             .Select( a => a.DoubleAddressDetails.Where( d => c.Contains(d.Product.ItemVolume) ) )
                                                             .Count(), c ) );
            }
        }

        public IEnumerable< (int scans, Category category) > GetScans ( IEnumerable< Category > categories )
        {
            if ( _actionPeriodMap.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            return categories.Select( c => (_actionPeriodMap.Keys
                                                    .Cast< ReceptionAction >()
                                                    .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Select( d => d.ScanQuantity ) )
                                                    .Count(), c ) );
        }

        public IEnumerable< (int count, Category category) > GetQuantity ( IEnumerable< Category > categories )
        {
            if ( _actionPeriodMap.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0, c));
                case OperationGroups.Reception:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< ReceptionAction >()
                                                             .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.ProductQuantity ) )
                                                             .Sum(), c ) );
                case OperationGroups.Inventory:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< InventoryAction >()
                                                             .Select( a => a.InventoryActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.ProductQuantity ) )
                                                             .Sum(), c ) );
                default:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< DoubleAddressAction >()
                                                             .Select( a => a.DoubleAddressDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.ProductQuantity ) )
                                                             .Sum(), c ) );
            }
        }

        public IEnumerable< (double count, Category category) > GetVolumes ( IEnumerable< Category > categories )
        {
            if ( _actionPeriodMap.Count == 0 ) { return categories.Select( c => (0.0, c)); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0.0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0.0, c));
                case OperationGroups.Reception:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< ReceptionAction >()
                                                             .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.Volume() ) )
                                                             .Sum(), c ) );
                case OperationGroups.Inventory:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< InventoryAction >()
                                                             .Select( a => a.InventoryActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.Volume() ) )
                                                             .Sum(), c ) );
                default:
                    return categories.Select( c => (_actionPeriodMap.Keys
                                                             .Cast< DoubleAddressAction >()
                                                             .Select( a => a.DoubleAddressDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.Volume() ) )
                                                             .Sum(), c ) );
            }
        }


        public IEnumerator< Period > GetEnumerator ()
        {
            return _actionPeriodMap.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }
    }
}
