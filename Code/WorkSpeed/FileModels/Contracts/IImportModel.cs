
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels.Contracts
{
    public interface IImportModel
    {
        IEntity Accept ( IImportModelVisitor visitor );
    }
}
