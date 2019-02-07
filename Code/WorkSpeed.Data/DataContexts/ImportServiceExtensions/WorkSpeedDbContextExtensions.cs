using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.ImportServiceExtensions
{
    public static class WorkSpeedDbContextExtensions
    {
        public static async Task< Product > GetProductAsync ( this WorkSpeedDbContext dbContext, Product product )
            => await dbContext.Products.FirstOrDefaultAsync( p => p.Id == product.Id );

        public static IQueryable< Product > GetProducts ( this WorkSpeedDbContext dbContext )
            => dbContext.Products.AsQueryable();

        public static async Task< Employee > GetEmployeeAsync ( this WorkSpeedDbContext dbContext, Employee employee )
            => await dbContext.Employees.FirstOrDefaultAsync( e => e.Id.Equals( employee.Id ) );

        public static IQueryable< Employee > GetEmployees ( this WorkSpeedDbContext dbContext )
            => dbContext.Employees.AsQueryable();
    }
}
