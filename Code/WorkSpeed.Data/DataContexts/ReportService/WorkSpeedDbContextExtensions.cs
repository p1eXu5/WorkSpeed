using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.ReportService
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
    }
}
