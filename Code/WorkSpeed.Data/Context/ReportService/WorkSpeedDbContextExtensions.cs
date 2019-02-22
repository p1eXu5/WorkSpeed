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
            var employees = dbContext.Employees.Include( e => e.Avatar ).AsQueryable();

            var query = (from e in employees
                         where e.IsActive
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
            var doubleAddressActions = dbContext.DoubleAddressActions
                                                .Include( a => a.DoubleAddressDetails )
                                                .Include( a => a.Employee )
                                                .Include( a => a.Operation )
                                                .Where( a => a.StartTime >= start && a.StartTime < end )
                                                .AsQueryable();

            var receptionActions = dbContext.ReceptionActions
                                            .Include( a => a.ReceptionActionDetails )
                                            .Include( a => a.Employee )
                                            .Include( a => a.Operation )
                                            .Where( a => a.StartTime >= start && a.StartTime < end )
                                            .AsQueryable();

            var inventoryActions = dbContext.InventoryActions
                                            .Include( a => a.InventoryActionDetails )
                                            .Include( a => a.Employee )
                                            .Include( a => a.Operation )
                                            .Where( a => a.StartTime >= start && a.StartTime < end )
                                            .AsQueryable();

            var shipmentActions = dbContext.ShipmentActions
                                           .Include( a => a.Employee )
                                           .Include( a => a.Operation )
                                           .Where( a => a.StartTime >= start && a.StartTime < end )
                                           .AsQueryable();

            var otherActions = dbContext.OtherActions
                                        .Include( a => a.Employee )
                                        .Include( a => a.Operation )
                                        .Where( a => a.StartTime >= start && a.StartTime < end )
                                        .AsQueryable();

            var set = new HashSet< EmployeeActionBase >();
            set.UnionWith( doubleAddressActions );
            set.UnionWith( receptionActions );
            set.UnionWith( shipmentActions );
            set.UnionWith( otherActions );

            return set.OrderByDescending( s => s.StartTime ).GroupBy( s => s.Employee );
        }
    }
}
