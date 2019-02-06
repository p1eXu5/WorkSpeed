using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts
{
    public static class WorkSpeedDbContextExtensions
    {
        public static void Add< TEntity > ( this WorkSpeedDbContext dbContext, TEntity entity ) where TEntity : class, IEntity
        {

        }
    }
}
