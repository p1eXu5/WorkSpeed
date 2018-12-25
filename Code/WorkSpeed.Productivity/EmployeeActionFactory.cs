using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class EmployeeActionFactory
    {
        private readonly Dictionary< string, EmployeeActionRepository > _actionRepositories;
        private readonly IPauseBetweenActions _pauseBetweenActions;
        private readonly ICategoryFilter _categoryFilter;

        public EmployeeActionFactory ( IPauseBetweenActions pause,  ICategoryFilter categoryFilter )
        {
            _pauseBetweenActions = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
            _categoryFilter = categoryFilter ?? throw new ArgumentNullException( nameof( categoryFilter ), "ICategoryFilter cannot be null." );

            _actionRepositories = new Dictionary< string, EmployeeActionRepository >();
        }

        public void AddAction ( EmployeeAction action )
        {
            if ( !_actionRepositories.ContainsKey( action.Employee.Id ) ) {
                _actionRepositories[ action.Employee.Id ] = new EmployeeActionRepository( action.Employee, _pauseBetweenActions, _categoryFilter );
            }

            _actionRepositories[ action.Employee.Id ].AddAction( action );
        }

        public EmployeeProductivity GetProductivity ( Employee employee )
        {
            throw new NotImplementedException();
        }
    }
}
