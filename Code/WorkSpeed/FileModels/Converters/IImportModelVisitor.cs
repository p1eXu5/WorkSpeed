
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels.Converters
{
    public interface IImportModelVisitor
    {
        IEntity GetDbModel ( ImportModel importModel );
    }
}