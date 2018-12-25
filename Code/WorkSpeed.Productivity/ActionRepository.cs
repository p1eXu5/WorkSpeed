using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class ActionRepository
    {
        private readonly Employee _employee;
        private EmployeeAction _lastAction;
        private IPauseBetweenActions _pause;

        public ActionRepository ( Employee employee, IPauseBetweenActions pause )
        {
            _employee = employee ?? throw new ArgumentNullException( nameof(employee), "Employee cannot be null." );
            _pause = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
        }

        public void AddAction ( EmployeeAction action )
        {
            _pause.Update( _lastAction, action );



            UpdateLastAction( action );
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
