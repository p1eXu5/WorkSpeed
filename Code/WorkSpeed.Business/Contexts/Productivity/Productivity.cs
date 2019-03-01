using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.Productivity
{
    /// <summary>
    ///     Created for each operation.
    /// </summary>
    public class Productivity : IProductivity
    {
        private readonly Dictionary< EmployeeActionBase, Period > _actions;

        #region Ctor

        public Productivity ()
        {
            _actions = new Dictionary< EmployeeActionBase, Period >();
        }

        #endregion

        public void Add ( EmployeeActionBase action, Period period )
        {
            _actions[ action ] = period;
        }

        public Period this [ EmployeeActionBase action ]
        {
            get => _actions[ action ];
            set => _actions[ action ] = value;
        }

        public TimeSpan  GetTime ()
        {
            return _actions.Values.Aggregate( TimeSpan.Zero, (acc, next) => acc + next.Duration );
        }

        public double GetLinesPerHour ()
        {
            if ( _actions.Count == 0 ) { return 0.0; }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return 0.0; }

            double lines = 0.0;

            switch ( group ) {
                case OperationGroups.Undefined:
                    return 0.0;
                case OperationGroups.Reception:
                    lines = _actions.Keys.Cast< ReceptionAction >().Sum( r => r.ReceptionActionDetails.Count );
                    break;
                case OperationGroups.Inventory:
                    lines = _actions.Keys.Cast< InventoryAction >().Sum( r => r.InventoryActionDetails.Count );
                    break;
                default:
                    lines = _actions.Keys.Cast< DoubleAddressAction >().Sum( r => r.DoubleAddressDetails.Count );
                    break;
            }

            return GetTotalHours() / lines;
        }

        public double GetTotalHours ()
        {
            if ( _actions.Count == 0 ) { return 0.0; }

            return _actions.Values.Sum( p => p.Duration.TotalHours );
        }

        public double GetScansPerHour ()
        {
            if ( _actions.Count == 0 ) { return 0.0; }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Reception ) { return 0.0; }

            var scans =  _actions.Keys.Cast< ReceptionAction >().Sum( r => r.ReceptionActionDetails.Sum( d => d.ScanQuantity ) );

            return GetTotalHours() / scans;
        }

        public double GetTotalVolume ()
        {
            if ( _actions.Count == 0 ) { return 0.0; }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Shipment ) { return 0.0; }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return 0.0;
                case OperationGroups.Shipment:
                    return _actions.Keys.Cast< ShipmentAction >().Sum( r => r.Volume ?? 0.0 );
                case OperationGroups.Reception:
                    return _actions.Keys.Cast< ReceptionAction >().Sum( r => r.ReceptionActionDetails.Sum( d => (Convert.ToDouble(d.Product.ItemVolume) * d.ProductQuantity) ) );
                case OperationGroups.Inventory:
                    return _actions.Keys.Cast< InventoryAction >().Sum( r => r.InventoryActionDetails.Sum( d => (Convert.ToDouble(d.Product.ItemVolume) * d.ProductQuantity) )  );
                default:
                    return _actions.Keys.Cast< DoubleAddressAction >().Sum( r => r.DoubleAddressDetails.Sum( d => (Convert.ToDouble(d.Product.ItemVolume) * d.ProductQuantity) )  );
            }
        }

        public (double clientCargo, double nonClientCargo) GetCargoQuantity ()
        {
            if ( _actions.Count == 0 ) { return (0.0, 0.0); }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Shipment ) { return (0.0, 0.0); }

            var clientCargo =  _actions.Keys.Cast< ShipmentAction >().Sum( r => Convert.ToDouble(r.ClientCargoQuantity) );
            var nonClientCargo =  _actions.Keys.Cast< ShipmentAction >().Sum( r =>  Convert.ToDouble(r.CommonCargoQuantity) );

            return (clientCargo, nonClientCargo);
        }

        public IEnumerable< (int count, Category category) > GetLines ( IEnumerable< Category > categories )
        {
            if ( _actions.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0, c));
                case OperationGroups.Reception:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< ReceptionAction >()
                                                             .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ) )
                                                             .Count(), c ) );
                case OperationGroups.Inventory:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< InventoryAction >()
                                                             .Select( a => a.InventoryActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ) )
                                                             .Count(), c ) );
                default:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< DoubleAddressAction >()
                                                             .Select( a => a.DoubleAddressDetails.Where( d => c.Contains(d.Product.ItemVolume) ) )
                                                             .Count(), c ) );
            }
        }

        public IEnumerable< (int scans, Category category) > GetScans ( IEnumerable< Category > categories )
        {
            if ( _actions.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            return categories.Select( c => (_actions.Keys
                                                    .Cast< ReceptionAction >()
                                                    .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Select( d => d.ScanQuantity ) )
                                                    .Count(), c ) );
        }

        public IEnumerable< (int count, Category category) > GetQuantity ( IEnumerable< Category > categories )
        {
            if ( _actions.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0, c));
                case OperationGroups.Reception:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< ReceptionAction >()
                                                             .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.ProductQuantity ) )
                                                             .Sum(), c ) );
                case OperationGroups.Inventory:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< InventoryAction >()
                                                             .Select( a => a.InventoryActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.ProductQuantity ) )
                                                             .Sum(), c ) );
                default:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< DoubleAddressAction >()
                                                             .Select( a => a.DoubleAddressDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => d.ProductQuantity ) )
                                                             .Sum(), c ) );
            }
        }

        public IEnumerable< (double count, Category category) > GetVolumes ( IEnumerable< Category > categories )
        {
            if ( _actions.Count == 0 ) { return categories.Select( c => (0.0, c)); }

            var group = _actions.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0.0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0.0, c));
                case OperationGroups.Reception:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< ReceptionAction >()
                                                             .Select( a => a.ReceptionActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => (Convert.ToDouble(d.Product.ItemVolume) * d.ProductQuantity) ) )
                                                             .Sum(), c ) );
                case OperationGroups.Inventory:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< InventoryAction >()
                                                             .Select( a => a.InventoryActionDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => (Convert.ToDouble(d.Product.ItemVolume) * d.ProductQuantity) ) )
                                                             .Sum(), c ) );
                default:
                    return categories.Select( c => (_actions.Keys
                                                             .Cast< DoubleAddressAction >()
                                                             .Select( a => a.DoubleAddressDetails.Where( d => c.Contains(d.Product.ItemVolume) ).Sum( d => (Convert.ToDouble(d.Product.ItemVolume) * d.ProductQuantity) ) )
                                                             .Sum(), c ) );
            }
        }


        public IEnumerator< Period > GetEnumerator ()
        {
            return _actions.Values.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }
    }
}
