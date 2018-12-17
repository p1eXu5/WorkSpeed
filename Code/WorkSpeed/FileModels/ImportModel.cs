using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public abstract class ImportModel
    {
        public virtual object Convert ( IImportModelVisitor visitor )
        {
            return visitor.GetDbModel( this );
        }
    }
}
