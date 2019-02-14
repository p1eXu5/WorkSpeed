using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Globalization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Tests.Contexts.UnitTests
{
    [ TestFixture ]
    public class ReportServiceTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-Us", false);
        }

        [ TearDown ]
        public void CloseConnection ()
        {
            _connection?.Close();
        }

        [ Test ]
        public void ReloadEmployees_DbHasEmployees_LoadsShiftGrouping ()
        {
            var service = GetImportService();

            service.LoadEmployees();

            Assert.That( service.Shifts, Is.Not.Empty );
        }

        [ Test ]
        public void ReloadEmployees_DbHasNotEmployees_DoesNotLoadShiftGrouping ()
        {
            var service = GetImportService( fillEmployees: false );

            service.LoadEmployees();

            Assert.That( service.Shifts, Is.Empty );
        }

        #region Factory


        private DbConnection _connection;

        private ReportService GetImportService ( bool fillEmployees = true )
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();

            var options = new DbContextOptionsBuilder< WorkSpeedDbContext >().UseSqlite( _connection ).Options;

            if ( fillEmployees ) { 
                using (var dbContext = new WorkSpeedDbContext(options) ) { 

                    dbContext.Database.EnsureCreated();

                    var shifts = dbContext.Shifts.ToArray();
                    var appointments = dbContext.Appointments.ToArray();
                    var positions = dbContext.Positions.ToArray();

                    dbContext.Employees.AddRange( new Employee[] {
                        new Employee { Id = "AR00001", Name = "Mnager", IsActive = true, Shift = shifts[0], Appointment = appointments[7], Position = positions[0] }, 
                        new Employee { Id = "AR00002", Name = "Manager Assistant 1", IsActive = true, Shift = shifts[0], Appointment = appointments[6], Position = positions[1] }, 
                        new Employee { Id = "AR00003", Name = "Manager Assistant 2", IsActive = true, Shift = shifts[0], Appointment = appointments[5], Position = positions[2] }, 
                        new Employee { Id = "AR00004", Name = "Principal Warehouseman 1", IsActive = true, Shift = shifts[0], Appointment = appointments[4], Position = positions[1] }, 
                        new Employee { Id = "AR00005", Name = "Principal Warehouseman 2", IsActive = true, Shift = shifts[0], Appointment = appointments[4], Position = positions[2] }, 
                        new Employee { Id = "AR00006", Name = "Principal Warehouseman 3", IsActive = true, Shift = shifts[1], Appointment = appointments[4], Position = positions[1] }, 
                    } );

                    dbContext.SaveChanges();
                };
            }
            var reportService = new ReportService( new WorkSpeedDbContext(options) );

            return reportService;
        }

        #endregion
    }
}
