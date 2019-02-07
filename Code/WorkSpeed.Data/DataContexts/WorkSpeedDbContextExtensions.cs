using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts
{
    public static class WorkSpeedDbContextExtensions
    {
        public static Task AddRangeAsync< TEntity > ( this WorkSpeedDbContext dbContext, IEnumerable< TEntity > entities ) 
            where TEntity : class, IEntity
        {
            return dbContext.Set< TEntity >().AddRangeAsync( entities );
        }

        public static void UpdateRange< TEntity > ( this WorkSpeedDbContext dbContext, IEnumerable< TEntity > entities )
            where TEntity : class, IEntity
        {
            dbContext.Set< TEntity >().UpdateRange( entities );
        }
    }
}
