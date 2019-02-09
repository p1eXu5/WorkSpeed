using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

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

        public static IQueryable< Appointment > GetAppointments ( this WorkSpeedDbContext dbContext )
            => dbContext.Appointments.AsQueryable();

        public static IQueryable< Rank > GetRanks ( this WorkSpeedDbContext dbContext )
            => dbContext.Ranks.AsQueryable();

        public static IQueryable< Position > GetPositions ( this WorkSpeedDbContext dbContext )
            => dbContext.Positions.AsQueryable();

        public static async Task< OtherAction > GetOtherActionAsync ( this WorkSpeedDbContext dbContext, OtherAction product )
            => await dbContext.OtherActions.FirstOrDefaultAsync( p => p.Id == product.Id );

        public static async Task< ShipmentAction > GetShipmentActionAsync ( this WorkSpeedDbContext dbContext, ShipmentAction product )
            => await dbContext.ShipmentActions.FirstOrDefaultAsync( p => p.Id == product.Id );

        public static async Task< InventoryAction > GetInventoryActionAsync ( this WorkSpeedDbContext dbContext, InventoryAction product )
            => await dbContext.InventoryActions.FirstOrDefaultAsync( p => p.Id == product.Id );

        public static async Task< ReceptionAction > GetReceptionActionAsync ( this WorkSpeedDbContext dbContext, ReceptionAction product )
            => await dbContext.ReceptionActions.FirstOrDefaultAsync( p => p.Id == product.Id );

        public static async Task< DoubleAddressAction > GetDoubleAddressActionAsync ( this WorkSpeedDbContext dbContext, DoubleAddressAction product )
            => await dbContext.DoubleAddressActions.FirstOrDefaultAsync( p => p.Id == product.Id );
    }
}
