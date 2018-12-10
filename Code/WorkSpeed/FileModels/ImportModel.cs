using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.ActionModels;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public abstract class ImportModel
    {
        public abstract Employee Employee { get; }
        public abstract IEnumerable<EmployeeAction> GetActions();
    }
}
