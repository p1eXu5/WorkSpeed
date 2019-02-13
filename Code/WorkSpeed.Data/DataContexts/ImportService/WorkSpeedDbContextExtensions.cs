using System;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.DataContexts.ImportService
{
    public static class WorkSpeedDbContextExtensions
    {

        public static IQueryable< Product > GetProducts ( this WorkSpeedDbContext dbContext )
            => dbContext.Products.AsQueryable();

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

        public static IQueryable< DoubleAddressAction > GetDoubleAddressActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.DoubleAddressActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static IQueryable< ReceptionAction > GetReceptionActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.ReceptionActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static IQueryable< InventoryAction > GetInventoryActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.InventoryActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static IQueryable< ShipmentAction > GetShipmentActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.ShipmentActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );

        public static IQueryable< OtherAction > GetOtherActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.OtherActions.Where( a => a.StartTime >= periodStart && a.StartTime < periodEnd );
    }
}
