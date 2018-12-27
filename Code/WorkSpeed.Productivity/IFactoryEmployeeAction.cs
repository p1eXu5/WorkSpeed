using System;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public interface IFactoryEmployeeAction
    {
        IPauseBetweenActions PauseBetweenActions { get; }
        ICategoryFilter CategoryFilter { get; }

        void AddAction ( EmployeeAction action );
        ProductivityEmployee GetProductivity ( Employee employee );

        void AddVariableBreak ( string name, TimeSpan breakDuration, DayPeriod dayPeriod );
        void AddFixedBreaks ( string name,
                              TimeSpan duration,
                              TimeSpan interval,
                              TimeSpan offset,
                              Predicate<Employee> predicate );
    }
}