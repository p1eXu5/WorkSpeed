using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.FileModels
{
    public interface IImportModelVisiter
    {
        EmployeeAction ToEmployeeAction ( ImportModel importModel );

    }

    public class ImportModelVisiter : IImportModelVisiter
    {
        public EmployeeAction ToEmployeeAction ( ImportModel importModel )
        {
            throw new NotImplementedException();
        }
    }
}
