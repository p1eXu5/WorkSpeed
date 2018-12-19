using NpoiExcel.Attributes;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class EmployeeImportModel : WithEmployeeImportModel
    {
        [Header( "Работает")]
        public bool IsActive { get; set; }

        public override object Convert ( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
