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
    }
}
