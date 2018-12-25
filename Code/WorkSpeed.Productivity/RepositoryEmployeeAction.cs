using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;
using WorkSpeed.Productivity.ActionDetails;

namespace WorkSpeed.Productivity
{
    public class RepositoryEmployeeAction
    {
        private EmployeeAction _lastAction;
        private readonly IPauseBetweenActions _pause;
        private TimeActionDetails[] _actions;

        public RepositoryEmployeeAction ( IPauseBetweenActions pause, ICategoryFilter categoryFilter )
        {
            _pause = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
            if ( categoryFilter == null ) throw new ArgumentNullException( nameof( categoryFilter ), "ICategoryFilter cannot be null." );

            InitActionDetailsArray( categoryFilter );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void AddAction ( EmployeeAction action )
        {
            var pause = _pause.GetPauseInterval( _lastAction, action );

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

            foreach ( var action in _actions ) {
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
        public Dictionary< OperationGroups, int[] > GetLines ()
        {
            var lines = new Dictionary<OperationGroups, int[]>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withProductActions ) {
                    lines[ ( OperationGroups )index ] = withProductActions.Lines.ToArray();
                }
            }

            return lines;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary< OperationGroups, int[] > GetQuantities ()
        {
            var quantities = new Dictionary<OperationGroups, int[]>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) )
            {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withProductActions )
                {
                    quantities[ ( OperationGroups )index ] = withProductActions.Quantity.ToArray();
                }
            }

            return quantities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary< OperationGroups, int[] > GetScans ()
        {
            var scans = new Dictionary<OperationGroups, int[]>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                if ( _actions[ index ] is WithScansActionDetails withScansActions )
                {
                    scans[ ( OperationGroups )index ] = withScansActions.Scans.ToArray();
                }
            }

            return scans;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<OperationGroups, double[]> GetWeights ()
        {
            var weights = new Dictionary<OperationGroups, double[]>();
            
            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) ) {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withScansActions ) {
                    weights[ ( OperationGroups )index ] = withScansActions.Weight.ToArray();
                }
                else if ( _actions[ index ] is ShipmentActionDetails shipmentActions ) {
                    weights[ ( OperationGroups )index ] = new double[] { shipmentActions.Weight };
                }
            }

            return weights;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<OperationGroups, double[]> GetVolumes ()
        {
            var volumes = new Dictionary<OperationGroups, double[]>();

            foreach ( var operation in Enum.GetValues( typeof( OperationGroups ) ) )
            {

                var index = ( int )operation;

                if ( _actions[ index ] is WithProductActionDetails withScansActions )
                {
                    volumes[ ( OperationGroups )index ] = withScansActions.Volume.ToArray();
                }
                else if ( _actions[ index ] is ShipmentActionDetails shipmentActions )
                {
                    volumes[ ( OperationGroups )index ] = new double[] { shipmentActions.Volume };
                }
            }

            return volumes;
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

            _actions[ ( int )OperationGroups.ClientGathering ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.ClientPacking ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.ClientScanning ] = new WithScansActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Defragmentation ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Gathering ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Inventory ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Packing ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Placing ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Replacing ] = new WithProductActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Scanning ] = new WithScansActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Shipment ] = new ShipmentActionDetails();
            _actions[ ( int )OperationGroups.ShopperGathering ] = new WithScansActionDetails( categoryFilter );
        }

        /// <summary>
        /// Updates _lastAction.
        /// </summary>
        /// <param name="action"></param>
        [ MethodImpl( MethodImplOptions.AggressiveInlining ) ]
        private void UpdateLastAction ( EmployeeAction action )
        {
            _lastAction = action;
        }
    }
}
