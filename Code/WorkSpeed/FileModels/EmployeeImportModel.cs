using Agbm.NpoiExcel.Attributes;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class EmployeeImportModel : WithEmployeeImportModel
    {
        [Header( "Работает")]
        public bool IsActive { get; set; }
    }
}
