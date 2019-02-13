using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.MainViewModel
{
    public static class WorkSpeedDbContextExtensions
    {
        public static IQueryable< Employee > GetActiveEmployees ( this WorkSpeedDbContext dbContext )
            => dbContext.Employees.Include( e => e.Shift ).Include( e => e.Appointment ).Include( e => e.Position ).Where( e => e.IsActive ).AsQueryable();

        public static IQueryable< Employee > GetInactiveEmployees ( this WorkSpeedDbContext dbContext )
            => dbContext.Employees.Include( e => e.Shift ).Include( e => e.Appointment ).Include( e => e.Position ).Where( e => !e.IsActive ).AsQueryable();
    }
}
