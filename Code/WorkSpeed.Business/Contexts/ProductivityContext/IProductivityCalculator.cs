﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.ActionModels;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.ProductivityCalculator
{
    public interface IProductivityCalculator<TAction> where TAction : EmployeeActionBase
    {
        void Calculate ( SortedSet< TAction > actions,  ICollection< Productivity2 > productivities );
    }
}
