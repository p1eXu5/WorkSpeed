using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiExcel.Attributes;

namespace WorkSpeed.FileModels
{
    public abstract class WithEmployeeImportModel : ImportModel
    {
        [ Header( "Код сотрудника" ) ]
        [ Header( "Код" ) ]
        public string EmployeeId { get; set; }

        [ Header( "Сотрудник" ) ]
        [ Header( "Кладовщик" ) ]
        public string EmployeeName { get; set; }
    }
}
