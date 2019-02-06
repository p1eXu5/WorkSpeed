using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels.Converters;

namespace WorkSpeed.Business.FileModels.Contracts
{
    public interface IImportModel
    {
        IEntity Convert ( IImportModelVisitor visitor );
    }
}
