﻿using System;
using System.Collections.Generic;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public interface IFactoryEmployeeAction
    {
        void AddAction ( EmployeeActionBase action );
        void ClearActions ();

        Productivity GetProductivity ( Employee employee );

        void AddVariableBreak ( Shift shift );

        void AddFixedBreaks ( ShortBreakSchedule shortBreak );

        IEnumerable< Category > GetCategories ();
        TimeSpan GetThreshold ();

        bool HasOperations ( Employee employee );

    }
}