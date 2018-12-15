using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Interfaces
{
    public interface IImportModelVisitor
    {
        object GetDbModel ( ImportModel importModel );
    }
}