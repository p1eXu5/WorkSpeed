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
    public class ActionRepository
    {
        private readonly Employee _employee;
        private EmployeeAction _lastAction;
        private IPauseBetweenActions _pause;
        private TimeActionDetails[] _actions;

        public ActionRepository ( Employee employee, IPauseBetweenActions pause, ICategoryFilter categoryFilter )
        {
            _employee = employee ?? throw new ArgumentNullException( nameof(employee), "Employee cannot be null." );
            _pause = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
            if ( categoryFilter == null ) throw new ArgumentNullException( nameof( categoryFilter ), "ICategoryFilter cannot be null." );

            InitActionDetailsArray( categoryFilter );
        }

        public void AddAction ( EmployeeAction action )
        {
            var pause = _pause.GetPauseInterval( _lastAction, action );

            var operation = ( int )action.GetOperationGroup();
            _actions[ operation ].AddDetails( action, pause );

            UpdateLastAction( action );
        }

        [ MethodImpl( MethodImplOptions.AggressiveInlining ) ]
        private void InitActionDetailsArray ( ICategoryFilter categoryFilter )
        {
            // Count of operation groups plus for off time.
            var operationCount = Enum.GetValues( typeof( OperationGroups ) ).Length + 1;

            _actions = new TimeActionDetails[ operationCount ];
            _actions[ operationCount  - 1 ] = new TimeActionDetails();

            _actions[ ( int )OperationGroups.ClientGathering ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.ClientPacking ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.ClientScanning ] = new WithScansActionDetails( categoryFilter );
            _actions[ ( int )OperationGroups.Defragmentation ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.Gathering ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.Inventory ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.Packing ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.Placing ] = new WithProductActionDtails( categoryFilter );
            _actions[ ( int )OperationGroups.Replacing ] = new WithProductActionDtails( categoryFilter );
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
