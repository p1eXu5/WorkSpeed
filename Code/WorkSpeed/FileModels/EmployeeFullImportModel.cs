using Agbm.NpoiExcel.Attributes;
using WorkSpeed.FileModels.Converters;

namespace WorkSpeed.FileModels
{
    public class EmployeeFullImportModel : EmployeeImportModel
    {
        [ Header(" Зона ")]         public string Position { get; set; }
        [ Header(" Должность ")]  public string Appointment { get; set; }
        [ Header(" Ранг ")]       public int Rank { get; set; }

        public override object Convert ( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
