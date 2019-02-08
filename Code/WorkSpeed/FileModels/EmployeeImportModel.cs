
using Agbm.NpoiExcel.Attributes;

namespace WorkSpeed.Business.FileModels
{
    public class EmployeeImportModel : WithEmployeeImportModel
    {
        [Header( "Работает")]     public bool IsActive { get; set; }
        [ Header(" Зона ")]       public string Position { get; set; }
        [ Header(" Должность ")]  public string Appointment { get; set; }
        [ Header(" Ранг ")]       public int? Rank { get; set; }
    }
}
