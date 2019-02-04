using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;
using WorkSpeed.Productivity.ActionDetails;

namespace WorkSpeed.Productivity
{
    public class RepositoryEmployeeAction
    {
        private readonly IPauseBetweenActions _pause;

        private readonly TimeSpan _pauseThreshold;
        private readonly Queue< TimeSpan > _pauses;

        private EmployeeActionBase _lastAction;
        private TimeActionDetails[] _actions;


        public RepositoryEmployeeAction ( IPauseBetweenActions pause, ICategoryFilter categoryFilter, TimeSpan pauseThreshold )
        {
            _pause = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
            if ( categoryFilter == null ) throw new ArgumentNullException( nameof( categoryFilter ), "ICategoryFilter cannot be null." );

            if ( pauseThreshold < TimeSpan.Zero ) throw new ArgumentException();

            _pauseThreshold = pauseThreshold;
            _pauses = new Queue< TimeSpan >();

            InitActionDetailsArray( categoryFilter );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void AddAction ( EmployeeActionBase action )
        {
            if ( _lastAction != null ) {

                var lastActionEnd = _lastAction.StartTime.Add( _lastAction.Duration );

                if ( lastActionEnd > action.StartTime ) {

                    var diff = lastActionEnd - action.StartTime;
                    _lastAction.Duration -= diff;

                    var lastOperation = ( int )_lastAction.GetOperationGroup();
                    _actions[ lastOperation ].Duration -= diff;
                }
            }

            var pause = _pause.GetPauseInterval( _lastAction, action );
            _pauses.Enqueue( pause );

            if ( pause > _pauseThreshold ) {

                // OffTime
                //_actions[ _actions.Length - 1 ].AddDetails( new EmployeeActionBase(), pause - _pauseThreshold );
                pause = _pauseThreshold;
            }

            if ( pause < TimeSpan.Zero ) pause = _pauseThreshold;

            var operation = ( int )action.GetOperationGroup();
            _actions[ operation ].AddDetails( action, pause );

            UpdateLastAction( action );
        }

        /// <summary>
        /// Returns total time.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalTime ()
        {
            var totalTime = TimeSpan.Zero;

            foreach ( var action in _actions.Where( a => a != null ) ) {
                totalTime += action.Duration;
            }

            return totalTime;
        }

        /// <summary>
        /// Returns off time.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetOffTime ()
        {
            return _actions[ _actions.Length - 1 ].Duration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary< OperationGroups, TimeSpan > GetOperationTimes ()
        {
            var operationTimes = new Dictionary<OperationGroups, TimeSpan>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                operationTimes[ ( OperationGroups )index ] = _actions[ index ].Duration;
            }

            return operationTimes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary< OperationGroups, Dictionary< Category, int >> GetLines ()
        {
            var lines = new Dictionary<OperationGroups, Dictionary<Category, int>>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withProductActions ) {
                    lines[ ( OperationGroups )index ] = withProductActions.GetLinesMap();
                }
            }

            return lines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary< OperationGroups, Dictionary< Category, int >> GetQuantities ()
        {
            var quantities = new Dictionary <OperationGroups, Dictionary< Category, int >>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) )
            {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withProductActions ) {

                    quantities[ ( OperationGroups )index ] = withProductActions.GetQuantityMap();
                }
            }

            return quantities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary< OperationGroups, Dictionary< Category, int >> GetScans ()
        {
            var scans = new Dictionary< OperationGroups, Dictionary< Category, int >>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                if ( _actions[ index ] is WithScansActionDetails withScansActions )
                {
                    scans[ ( OperationGroups )index ] = withScansActions.GetScansMap();
                }
            }

            return scans;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<OperationGroups, Dictionary< Category, double >> GetWeights ()
        {
            var weights = new Dictionary<OperationGroups, Dictionary<Category, double>>();
            
            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withScansActions ) {

                    weights[ ( OperationGroups )index ] = withScansActions.GetWeightMap();
                }
                else if ( _actions[ index ] is ShipmentActionDetails shipmentActions ) {

                    var map = new Dictionary<Category, double>();
                    map.Add( new Category( 0.0, double.PositiveInfinity ) { Name = "Без категорий" }, shipmentActions.Weight );

                    weights[ ( OperationGroups )index ] = map;
                }
            }

            return weights;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<OperationGroups, Dictionary<Category, double>> GetVolumes ()
        {
            var volumes = new Dictionary<OperationGroups, Dictionary< Category, double >>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) )
            {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails productActionDetails ) {

                    volumes[ ( OperationGroups )index ] = productActionDetails.GetVolumeMap();
                }
                else if ( _actions[ index ] is ShipmentActionDetails shipmentActions ) {

                    var map = new Dictionary< Category, double >();
                    map.Add( new Category( 0.0, double.PositiveInfinity ) { Name = "Без категорий" }, shipmentActions.Volume );

                    volumes[ ( OperationGroups )index ] = map;
                }
            }

            return volumes;
        }

        public Dictionary< TimeSpan, int > GetPauses ()
        {
            var pauses = new Dictionary< TimeSpan, int >(6);
            pauses[ TimeSpan.FromSeconds( 10 ) ] = _pauses.Count( p => p.TotalSeconds < 10 );
            pauses[ TimeSpan.FromSeconds( 30 ) ] = _pauses.Count( p => p.TotalSeconds >= 10 && p.TotalSeconds < 30 );
            pauses[ TimeSpan.FromSeconds( 60 ) ] = _pauses.Count( p => p.TotalSeconds >= 30 && p.TotalSeconds < 60 );
            pauses[ TimeSpan.FromMinutes( 2 ) ] = _pauses.Count( p => p.TotalSeconds >= 60 && p.TotalMinutes < 2 );
            pauses[ TimeSpan.FromMinutes( 5 ) ] = _pauses.Count( p => p.TotalMinutes >= 2 && p.TotalMinutes < 5 );
            pauses[ TimeSpan.FromMinutes( 60 ) ] = _pauses.Count( p => p.TotalMinutes >= 5 && p.TotalMinutes < 60 );

            return pauses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryFilter"></param>
        [ MethodImpl( MethodImplOptions.AggressiveInlining ) ]
        private void InitActionDetailsArray ( ICategoryFilter categoryFilter )
        {
            // Count of operation groups plus for off time.
            var operationCount = Enum.GetValues( typeof( OperationGroups ) ).Length + 1;

            _actions = new TimeActionDetails[ operationCount ];

            // OffTime
            _actions[ operationCount  - 1 ] = new TimeActionDetails();

            //_actions[ ( int )OperationGroups.ClientGathering ] = new WithProductActionDetails( categoryFilter );
            //_actions[ ( int )OperationGroups.ClientPacking ] = new WithProductActionDetails( categoryFilter );
            //_actions[ ( int )OperationGroups.ClientScanning ] = new WithScansActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Defragmentation ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Gathering ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Inventory ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Packing ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Placing ] = new WithProductActionDetails( categoryFilter );
            //_actions[ ( int )OperationGroups.Replacing ] = new WithProductActionDetails( categoryFilter );
           // _actions[ ( int )OperationGroups.Scanning ] = new WithScansActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Shipment ] = new ShipmentActionDetails();
            //_actions[ ( int )OperationGroups.ShopperGathering ] = new WithScansActionDetails( categoryFilter );
        }

        /// <summary>
        /// Updates _lastAction.
        /// </summary>
        /// <param name="action"></param>
        [ MethodImpl( MethodImplOptions.AggressiveInlining ) ]
        private void UpdateLastAction ( EmployeeActionBase action )
        {
            _lastAction = action;
        }
    }
}
