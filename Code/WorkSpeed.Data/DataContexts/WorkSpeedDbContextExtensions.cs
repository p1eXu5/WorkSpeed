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
        public static Task AddAsync< TEntity > ( this WorkSpeedDbContext dbContext, 
                                                 IEnumerable< TEntity > entities, 
                                                 CancellationToken cancellationToken ) 
            where TEntity : class, IEntity
        {
            return dbContext.Set< TEntity >().AddRangeAsync( entities, cancellationToken );
        }
    }
}
