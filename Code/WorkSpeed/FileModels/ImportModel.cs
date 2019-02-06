
using WorkSpeed.Business.FileModels.Contracts;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.FileModels
{
    public abstract class ImportModel : IImportModel
    {
        public virtual IEntity Convert ( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
