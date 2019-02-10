using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Productivity
{
    public class FactoryEmployeeAction : IFactoryEmployeeAction
    {
        private readonly Dictionary< string, RepositoryEmployeeAction > _actionRepositories;
        private readonly TimeSpan _pauseThreshold;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pause"></param>
        /// <param name="categoryFilter"></param>
        /// <param name="pauseThreshold"></param>
        public FactoryEmployeeAction ( IPauseBetweenActions pause,  ICategoryFilter categoryFilter, TimeSpan pauseThreshold )
        {
            PauseBetweenActions = pause ?? throw new ArgumentNullException( nameof( pause ), "IPauseBetweenActions cannot be null." );
            CategoryFilter = categoryFilter ?? throw new ArgumentNullException( nameof( categoryFilter ), "ICategoryFilter cannot be null." );

            _actionRepositories = new Dictionary< string, RepositoryEmployeeAction >();

            if ( pauseThreshold < TimeSpan.Zero ) throw new ArgumentException();

            _pauseThreshold = pauseThreshold;
        }

        public IPauseBetweenActions PauseBetweenActions { get; }

        public ICategoryFilter CategoryFilter { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void AddAction ( EmployeeActionBase action )
        {
            if ( !_actionRepositories.ContainsKey( action.Employee.Id ) ) {
                _actionRepositories[ action.Employee.Id ] = new RepositoryEmployeeAction( PauseBetweenActions, CategoryFilter, _pauseThreshold );
            }

            _actionRepositories[ action.Employee.Id ].AddAction( action );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public bool HasOperations ( Employee employee )
        {
            if ( employee == null ) throw new ArgumentNullException( nameof( employee ), "Employee cannot be null.");
            return _actionRepositories.ContainsKey( employee.Id );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public Productivity GetProductivity ( Employee employee )
        {
            if ( !HasOperations( employee ) ) return new Productivity();

            var actionRepository = _actionRepositories[ employee.Id ];

            var productivity = new Productivity {

                Employee = employee,
                TotalTime = actionRepository.GetTotalTime(),
                OffTime = actionRepository.GetOffTime(),
                OperationTimes = actionRepository.GetOperationTimes(),
                Lines = actionRepository.GetLines(),
                Quantities = actionRepository.GetQuantities(),
                Scans = actionRepository.GetScans(),
                Weight = actionRepository.GetWeights(),
                Volume = actionRepository.GetVolumes(),
                Pauses = actionRepository.GetPauses()
            };

            return productivity;
        }

        public void AddVariableBreak ( Shift shift )
        {
            PauseBetweenActions.BreakRepository.AddVariableBreak( shift );
        }

        public void AddFixedBreaks ( ShortBreakSchedule shortBreak )
        {
            PauseBetweenActions.BreakRepository.AddFixedBreak( shortBreak, ( e ) => !e.IsSmoker ?? false );
        }

        public IEnumerable< Category > GetCategories ()
        {
            return CategoryFilter.CategoryList;
        }
        public TimeSpan GetThreshold () => PauseBetweenActions.MinRestBetweenShifts;

        public void ClearActions ()
        {
            _actionRepositories.Clear();
        }
    }
}
