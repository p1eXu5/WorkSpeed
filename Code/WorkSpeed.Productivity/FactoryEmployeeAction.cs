using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public class FactoryEmployeeAction
    {
        private readonly Dictionary< string, RepositoryEmployeeAction > _actionRepositories;
        private readonly IPauseBetweenActions _pauseBetweenActions;
        private readonly ICategoryFilter _categoryFilter;

        public FactoryEmployeeAction ( IPauseBetweenActions pause,  ICategoryFilter categoryFilter )
        {
            _pauseBetweenActions = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
            _categoryFilter = categoryFilter ?? throw new ArgumentNullException( nameof( categoryFilter ), "ICategoryFilter cannot be null." );

            _actionRepositories = new Dictionary< string, RepositoryEmployeeAction >();
        }

        public void AddAction ( EmployeeAction action )
        {
            if ( !_actionRepositories.ContainsKey( action.Employee.Id ) ) {
                _actionRepositories[ action.Employee.Id ] = new RepositoryEmployeeAction( action.Employee, _pauseBetweenActions, _categoryFilter );
            }

            _actionRepositories[ action.Employee.Id ].AddAction( action );
        }

        public bool HasOperations ( Employee employee )
        {
            if ( employee == null ) throw new ArgumentNullException( nameof( employee ), "Employee cannot be null.");
            return _actionRepositories.ContainsKey( employee.Id );
        }

        public ProductivityEmployee GetProductivity ( Employee employee )
        {
            if ( !HasOperations( employee ) ) return new ProductivityEmployee();

            var actionRepository = _actionRepositories[ employee.Id ];

            var productivity = new ProductivityEmployee {

                Employee = employee,
                TotalTime = actionRepository.GetTotalTime(),
                OffTime = actionRepository.GetOffTime(),
                OperationTimes = actionRepository.GetOperationTimes(),
                Lines = actionRepository.GetLines(),
                Quantities = actionRepository.GetQuantities(),
                Scans = actionRepository.GetScans(),
                Weight = actionRepository.GetWeights(),
                Volume = actionRepository.GetVolumes()
            };

            return productivity;
        }
    }
}
