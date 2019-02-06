using Agbm.NpoiExcel.Attributes;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

namespace WorkSpeed.Business.FileModels
{
    public class EmployeeImportModel : WithEmployeeImportModel
    {
        [Header( "Работает")]   public bool IsActive { get; set; }
    }
}
