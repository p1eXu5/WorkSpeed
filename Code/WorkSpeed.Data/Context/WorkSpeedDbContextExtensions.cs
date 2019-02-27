using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context
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

        public static IQueryable< Appointment > GetAppointments ( this WorkSpeedDbContext dbContext )
            => dbContext.Appointments.OrderBy( a => a.Id ).AsQueryable();

        public static IQueryable< Rank > GetRanks ( this WorkSpeedDbContext dbContext )
            => dbContext.Ranks.OrderBy( r => r.Number ).AsQueryable();

        public static IQueryable< Position > GetPositions ( this WorkSpeedDbContext dbContext )
            => dbContext.Positions.OrderBy( p => p.Id ).AsQueryable();
    }
}
