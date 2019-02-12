using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.Business.FileModels
{
    public class EmployeeShortImportModel : WithEmployeeImportModel
    {
        [ Header( "Работает")]    public bool IsActive { get; set; }
    }
}
