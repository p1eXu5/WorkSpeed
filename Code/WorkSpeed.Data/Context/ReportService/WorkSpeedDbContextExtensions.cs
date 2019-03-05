using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Context.ReportService
{
    public static class WorkSpeedDbContextExtensions
    {
        public static IEnumerable<
                (Shift shift, ( Appointment appointment, (Position position, Employee[] employees)[] positions)[] appointments) >
        GetShiftGrouping ( this WorkSpeedDbContext dbContext )
        {
            foreach (var entityEntry in dbContext.ChangeTracker.Entries< Employee >()) {
                entityEntry.Reload();
            }

            var employees = dbContext.Employees.Include( e => e.Avatar ).ToArray();

            var query = (from e in employees
                         group e by e.Shift
                         into s
                         select new
                         {
                             Shift = s.Key,
                             Appointments = from e in s
                                            group e by e.Appointment
                                            into a
                                            select new
                                            {
                                                Appointment = a.Key,
                                                Positions = from e in a
                                                            group e by e.Position
                                                            into p
                                                            select new
                                                            {
                                                                Position = p.Key,
                                                                Employees = p
                                                            }
                                            }
                         }).AsQueryable();

            return query.AsEnumerable().Select( s => (
                                                       s.Shift,
                                                       s.Appointments?.Select( a => 
                                                                ( a.Appointment, 
                                                                  a.Positions.Select( p => 
                                                                        ( p.Position, p.Employees?.ToArray() ) ).ToArray() ) ).ToArray()
                                                   ));
        }

        public static IQueryable< Operation > GetOperations ( this WorkSpeedDbContext dbContext )
            => dbContext.Operations.AsQueryable();

        public static IQueryable< Employee > GetInactiveEmployees ( this WorkSpeedDbContext dbContext )
            => dbContext.Employees.Include( e => e.Shift ).Include( e => e.Appointment ).Include( e => e.Position ).Where( e => !e.IsActive ).AsQueryable();

        public static IEnumerable< IGrouping< Employee, EmployeeActionBase >> GetEmployeeActions ( this WorkSpeedDbContext dbContext, DateTime start, DateTime end )
        {
            foreach (var entityEntry in dbContext.ChangeTracker.Entries< Employee >()) {
                entityEntry.Reload();
            }

            foreach (var entityEntry in dbContext.ChangeTracker.Entries< Product >()) {
                entityEntry.Reload();
            }

            var doubleAddressActions = dbContext.DoubleAddressActions
                                                .Include( a => a.Employee )
                                                .ThenInclude( e => e.Avatar )
                                                .Include( a => a.Operation )
                                                .Include( a => a.DoubleAddressDetails )
                                                .ThenInclude( d => d.Product )
                                                .Where( a => a.StartTime >= start && a.StartTime < end && a.Duration > TimeSpan.Zero )
                                                .AsQueryable();

            var receptionActions = dbContext.ReceptionActions
                                            .Include( a => a.Employee )
                                            .ThenInclude( e => e.Avatar )
                                            .Include( a => a.Operation )
                                            .Include( a => a.ReceptionActionDetails )
                                            .ThenInclude( d => d.Product )
                                            .Where( a => a.StartTime >= start && a.StartTime < end && a.Duration > TimeSpan.Zero )
                                            .AsQueryable();

            var inventoryActions = dbContext.InventoryActions
                                            .Include( a => a.Employee )
                                            .ThenInclude( e => e.Avatar )
                                            .Include( a => a.Operation )
                                            .Include( a => a.InventoryActionDetails )
                                            .ThenInclude( d => d.Product )
                                            .Where( a => a.StartTime >= start && a.StartTime < end && a.Duration > TimeSpan.Zero )
                                            .AsQueryable();

            var shipmentActions = dbContext.ShipmentActions
                                           .Include( a => a.Employee )
                                           .ThenInclude( e => e.Avatar )
                                           .Include( a => a.Operation )
                                           .Where( a => a.StartTime >= start && a.StartTime < end && a.Duration > TimeSpan.Zero )
                                           .AsQueryable();

            var otherActions = dbContext.OtherActions
                                        .Include( a => a.Employee )
                                        .ThenInclude( e => e.Avatar )
                                        .Include( a => a.Operation )
                                        .Where( a => a.StartTime >= start && a.StartTime < end && a.Duration > TimeSpan.Zero )
                                        .AsQueryable();

            var set = new HashSet< EmployeeActionBase >();
            set.UnionWith( doubleAddressActions );
            set.UnionWith( receptionActions );
            set.UnionWith( inventoryActions );
            set.UnionWith( shipmentActions );
            set.UnionWith( otherActions );

            return set.OrderByDescending( s => s.StartTime ).GroupBy( s => s.Employee );
        }

        public static IQueryable< Shift > GetShifts ( this WorkSpeedDbContext dbContext )
            => dbContext.Shifts.AsQueryable();

        public static IQueryable< ShortBreakSchedule > GetShortBreakSchedules ( this WorkSpeedDbContext dbContext )
            => dbContext.ShortBreakSchedules.AsQueryable();

        public static IEnumerable< Category > GetCategories ( this WorkSpeedDbContext dbContext )
            => dbContext.CategorySets.Include( c => c.CategoryCategorySets ).ThenInclude( ccs => ccs.Category ).First().CategoryCategorySets.Select( ccs => ccs.Category );
    }
}
