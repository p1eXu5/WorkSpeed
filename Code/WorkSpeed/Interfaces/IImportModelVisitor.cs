using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Interfaces
{
    public interface IImportModelVisitor
    {
        EmployeeAction ToEmployeeAction ( ImportModel importModel );
        Product ToProduct( ProductImportModel         importModel );
    }
}