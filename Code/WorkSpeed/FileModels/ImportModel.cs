using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.FileModels.Contracts;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

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
