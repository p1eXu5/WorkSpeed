using System;
using System.Collections.Generic;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Productivity
{
    public interface IFactoryEmployeeAction
    {
        void AddAction ( EmployeeAction action );
        void ClearActions ();

        ProductivityEmployee GetProductivity ( Employee employee );

        void AddVariableBreak ( string name, TimeSpan breakDuration, DayPeriod dayPeriod );

        void AddFixedBreaks ( string name,
                              TimeSpan duration,
                              TimeSpan interval,
                              TimeSpan offset,
                              Predicate<Employee> predicate );

        IEnumerable< Category > GetCategories ();
        double GetThreshold ();
        void SetThreshold ( double threshold );
    }
}