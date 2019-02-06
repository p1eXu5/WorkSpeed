using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Business.FileModels;

namespace WorkSpeed.Business.FileModels
{
    public abstract class WithEmployeeImportModel : ImportModel
    {
        [ Header( "Код сотрудника" ) ]
        [ Header( "Код" ) ]
                                        public string Id { get; set; }

        [ Header( "Сотрудник" ) ]
        [ Header( "Кладовщик" ) ]
                                        public string Name { get; set; }
    }
}
