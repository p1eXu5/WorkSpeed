using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

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



        public static IQueryable< Operation > GetOperations ( this WorkSpeedDbContext dbContext, OperationGroups group )
            => dbContext.Operations.Where( o => o.OperationGroup == group ).AsQueryable();



        public static IQueryable< Address > GetAddresses ( this WorkSpeedDbContext dbContext )
            => dbContext.Addresses.AsQueryable();

        public static async Task< DoubleAddressAction > GetDoubleAddressActionAsync ( this WorkSpeedDbContext dbContext, DoubleAddressAction action )
            => await dbContext.DoubleAddressActions.FirstOrDefaultAsync( a => a.Id.Equals( action.Id ) );

        public static IQueryable< DoubleAddressAction > GetDoubleAddressActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.DoubleAddressActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static async Task< ReceptionAction > GetReceptionActionAsync ( this WorkSpeedDbContext dbContext, ReceptionAction action )
            => await dbContext.ReceptionActions.FirstOrDefaultAsync( a => a.Id.Equals( action.Id ) );

        public static IQueryable< ReceptionAction > GetReceptionActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.ReceptionActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static async Task< InventoryAction > GetInventoryActionAsync ( this WorkSpeedDbContext dbContext, InventoryAction action )
            => await dbContext.InventoryActions.FirstOrDefaultAsync( a => a.Id.Equals( action.Id ) );

        public static IQueryable< InventoryAction > GetInventoryActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.InventoryActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static async Task< ShipmentAction > GetShipmentActionAsync ( this WorkSpeedDbContext dbContext, ShipmentAction action )
            => await dbContext.ShipmentActions.FirstOrDefaultAsync( a => a.Id.Equals( action.Id ) );

        public static IQueryable< ShipmentAction > GetShipmentActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.ShipmentActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static async Task< OtherAction > GetOtherActionAsync ( this WorkSpeedDbContext dbContext, OtherAction action )
            => await dbContext.OtherActions.FirstOrDefaultAsync( a => a.Id.Equals( action.Id ) );

        public static IQueryable< OtherAction > GetOtherActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.OtherActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );
    }
}
