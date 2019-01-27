using System;
using System.Collections.Generic;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Productivity
{
    public interface IFactoryEmployeeAction
    {
        void AddAction ( EmployeeActionBase action );
        void ClearActions ();

        ProductivityEmployee GetProductivity ( Employee employee );

        void AddVariableBreak ( Shift shift );

        void AddFixedBreaks ( ShortBreakSchedule shortBreak );

        IEnumerable< Category > GetCategories ();
        TimeSpan GetThreshold ();

        bool HasOperations ( Employee employee );

    }
}