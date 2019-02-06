using WorkSpeed.Business.FileModels.Contracts;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Business.FileModels.Converters
{
    public interface IImportModelVisitor
    {
        IEntity GetDbModel ( ImportModel importModel );
    }
}