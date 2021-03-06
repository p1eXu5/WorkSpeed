﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.Context.ImportService
{
    public static class WorkSpeedDbContextExtensions
    {

        public static IQueryable< Product > GetProducts ( this WorkSpeedDbContext dbContext )
            => dbContext.Products.AsQueryable();

        public static IQueryable< Employee > GetEmployees ( this WorkSpeedDbContext dbContext )
            => dbContext.Employees.AsQueryable();


        public static Position GetDefaultPosition ( this WorkSpeedDbContext dbContext )
            => dbContext.Positions.First( s => s.Id == 1 );

        public static Rank GetDefaultRank ( this WorkSpeedDbContext dbContext )
            => dbContext.Ranks.First( s => s.Number == 3 );

        public static Appointment  GetDefaultAppointment ( this WorkSpeedDbContext dbContext )
            => dbContext.Appointments.First( s => s.Id == 1 );

        public static Shift  GetDefaultShift ( this WorkSpeedDbContext dbContext )
            => dbContext.Shifts.First( s => s.Id == 1 );

        public static ShortBreakSchedule GetDefaultShortBreakSchedule ( this WorkSpeedDbContext dbContext )
            => dbContext.ShortBreakSchedules.First( s => s.Id == 1 );

        public static Avatar GetDefaultAvatar ( this WorkSpeedDbContext dbContext )
            => dbContext.Avatars.First( a => a.Id == 1 );




        public static IQueryable< Operation > GetOperations ( this WorkSpeedDbContext dbContext, ISet< OperationGroups > group )
            => dbContext.Operations.Where( o => group.Contains( o.Group ) ).AsQueryable();

        public static IQueryable< Operation > GetOperations ( this WorkSpeedDbContext dbContext, OperationGroups group )
            => dbContext.Operations.Where( o => o.Group == group ).AsQueryable();


        public static IQueryable< Address > GetAddresses ( this WorkSpeedDbContext dbContext )
            => dbContext.Addresses.AsQueryable();


        public static IQueryable< DoubleAddressAction > GetDoubleAddressActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.DoubleAddressActions.Where( a => a.StartTime >= periodStart && a.StartTime <= periodEnd );

        public static IQueryable< ReceptionAction > GetReceptionActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.ReceptionActions.Where( a => a.StartTime >= periodStart && a.StartTime <= periodEnd );

        public static IQueryable< InventoryAction > GetInventoryActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.InventoryActions.Where( a => a.StartTime >= periodStart && a.StartTime <= periodEnd );

        public static IQueryable< ShipmentAction > GetShipmentActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.ShipmentActions.Where( a => a.StartTime >= periodStart && a.StartTime <= periodEnd );

        public static IQueryable< OtherAction > GetOtherActions ( this WorkSpeedDbContext dbContext, DateTime periodStart, DateTime periodEnd )
            => dbContext.OtherActions.Where( a => a.StartTime >= periodStart && a.StartTime <= periodEnd );
    }
}
