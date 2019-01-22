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

        void AddVariableBreak ( Shift shift );

        void AddFixedBreaks ( ShortBreakSchedule shortBreak );

        IEnumerable< Category > GetCategories ();
        TimeSpan GetThreshold ();

        bool HasOperations ( Employee employee );

    }
}