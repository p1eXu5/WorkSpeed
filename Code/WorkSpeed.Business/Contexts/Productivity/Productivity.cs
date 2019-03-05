using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.ActionDetails;
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

            List<(int count, Category category)> count = new List< (int count, Category category) >( categories.Count() );
            HashSet< Product > products;

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0, c));

                case OperationGroups.Reception:
                    return GetLines( _actionPeriodMap.Keys.Cast< ReceptionAction >(), categories, o => o.ReceptionActionDetails );

                case OperationGroups.Inventory:
                    return GetLines( _actionPeriodMap.Keys.Cast< InventoryAction >(), categories, o => o.InventoryActionDetails );

                default:
                    return GetLines( _actionPeriodMap.Keys.Cast< DoubleAddressAction >(), categories, o => o.DoubleAddressDetails );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actions"></param>
        /// <param name="categories"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        private IEnumerable< (int count, Category category) > GetLines< T > ( IEnumerable< T > actions, 
                                                                              IEnumerable< Category > categories, 
                                                                              Func< T, IEnumerable<WithProductActionDetail >> getter )
        {
            var products = new HashSet< Product >( (from a in actions
                                                    from d in getter( a )
                                                    select d.Product
                                                    ) );
            var dict = new Dictionary< Category, int >();

            foreach ( var category in categories ) {

                dict[ category ] = 0;

                if ( products.Any() ) {
                    foreach ( var product in products.ToArray() ) {
                        if ( category.Contains( product.ItemVolume ) ) {
                            products.Remove( product );
                            dict[ category ]++;
                        }
                    }
                }
            }

            return dict.Keys.Select( cat => ( dict[ cat ], cat) );
        }

        public IEnumerable< (int scans, Category category) > GetScans ( IEnumerable< Category > categories )
        {
            if ( _actionPeriodMap.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group != ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            var productScans = new HashSet< (Product product, short scans) >( (from a in _actionPeriodMap.Keys.Cast< ReceptionAction >()
                                                                               from d in a.ReceptionActionDetails
                                                                               select (d.Product, d.ScanQuantity)
                                                                               ) );
            var dict = new Dictionary< Category, int >();

            foreach ( var category in categories ) {

                dict[ category ] = 0;

                if ( productScans.Any() ) {
                    foreach ( var prodScan in productScans.ToArray() ) {
                        if ( category.Contains( prodScan.product.ItemVolume ) ) {
                            productScans.Remove( prodScan );
                            dict[ category ]+= prodScan.scans;
                        }
                    }
                }
            }

            return dict.Keys.Select( cat => ( dict[ cat ], cat) );
        }

        public IEnumerable< (int count, Category category) > GetQuantities ( IEnumerable< Category > categories )
        {
            if ( _actionPeriodMap.Count == 0 ) { return categories.Select( c => (0, c)); }

            var group = _actionPeriodMap.Keys.First().Operation.Group;
            if ( (int)group > ( int )OperationGroups.Reception ) { return categories.Select( c => (0, c)); }

            switch ( group ) {
                case OperationGroups.Undefined:
                    return categories.Select( c => (0, c));

                case OperationGroups.Reception:
                    return GetQuantities( _actionPeriodMap.Keys.Cast< ReceptionAction >(), categories, action => action.ReceptionActionDetails );

                case OperationGroups.Inventory:
                    return GetQuantities( _actionPeriodMap.Keys.Cast< InventoryAction >(), categories, action => action.InventoryActionDetails );

                default:
                    return GetQuantities( _actionPeriodMap.Keys.Cast< DoubleAddressAction >(), categories, action => action.DoubleAddressDetails );                
            }
        }

        private IEnumerable< (int count, Category category) > GetQuantities< T > ( IEnumerable< T > actions,
                                                                                 IEnumerable< Category > categories,
                                                                                 Func< T, IEnumerable< WithProductActionDetail > > getter )
        {
            var productQuantities = new HashSet< (Product product, int quantity) >( (from a in actions
                                                                                from d in getter(a)
                                                                                select (d.Product, d.ProductQuantity)
                                                                                ) );
            var dict = new Dictionary< Category, int >();

            foreach ( var category in categories ) {

                dict[ category ] = 0;

                if ( productQuantities.Any() ) {
                    foreach ( var prodQuant in productQuantities.ToArray() ) {
                        if ( category.Contains( prodQuant.product.ItemVolume ) ) {
                            productQuantities.Remove( prodQuant );
                            dict[ category ]+= prodQuant.quantity;
                        }
                    }
                }
            }

            return dict.Keys.Select( cat => ( dict[ cat ], cat) );
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
                    return GetVolumes( _actionPeriodMap.Keys.Cast< ReceptionAction >(), categories, action => action.ReceptionActionDetails );

                case OperationGroups.Inventory:
                    return GetVolumes( _actionPeriodMap.Keys.Cast< InventoryAction >(), categories, action => action.InventoryActionDetails );

                default:
                    return GetVolumes( _actionPeriodMap.Keys.Cast< DoubleAddressAction >(), categories, action => action.DoubleAddressDetails );
            }
        }

        private IEnumerable< (double count, Category category) > GetVolumes< T > ( IEnumerable< T > actions,
                                                                                   IEnumerable< Category > categories,
                                                                                   Func< T, IEnumerable< WithProductActionDetail > > getter )
        {
            var productQuantities = new HashSet< (Product product, double volume) >( from a in actions
                                                                                     from d in getter(a)
                                                                                     select (d.Product, d.Volume()) );
            var dict = new Dictionary< Category, double >();

            foreach ( var category in categories ) {

                dict[ category ] = 0;

                if ( productQuantities.Any() ) {
                    foreach ( var prodQuant in productQuantities.ToArray() ) {
                        if ( category.Contains( prodQuant.product.ItemVolume ) ) {
                            productQuantities.Remove( prodQuant );
                            dict[ category ]+= prodQuant.volume;
                        }
                    }
                }
            }

            return dict.Keys.Select( cat => ( dict[ cat ], cat) );
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
