using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public abstract class ImportModel
    {
        public abstract Employee GetEmployee();
        public abstract EmployeeAction GetAction();

        public virtual EmployeeAction ToEmployeeAction ( IImportModelVisiter visiter )
        {
            return visiter.ToEmployeeAction( this );
        }
    }
}
