using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Globalization;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Contexts.Productivity.Builders;
using WorkSpeed.Data.Context;
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

            if ( File.Exists( ReportService.THRESHOLD_FILE ) ) {
                File.Delete( ReportService.THRESHOLD_FILE );
            }
        }

        [ Test ]
        public void ReloadEmployees_DbHasEmployees_LoadsShiftGrouping ()
        {
            var service = GetReportService();

            service.LoadEmployeesAsync().Wait();

            Assert.That( service.ShiftGrouping, Is.Not.Empty );
        }

        [ Test ]
        public void ReloadEmployees_DbHasNotEmployees_DoesNotLoadShiftGrouping ()
        {
            var service = GetReportService( fillEmployees: false );

            service.LoadEmployeesAsync().Wait();

            Assert.That( service.ShiftGrouping, Is.Empty );
        }


        [ Test, Category( "GetThreshold" ) ]
        public void GetThresholds_ThresholdNotExists_ReturnsThresholds ()
        {
            var service = GetReportService( false );

            var thresholds = service.GetThresholds();

            Assert.That( thresholds, Is.Not.Null );
        }

        [ Test, Category( "GetThreshold" ) ]
        public void GetThresholds_ThresholdNotExists_CreatesBinaryFile ()
        {
            var service = GetReportService( false );

            var thresholds = service.GetThresholds();

            Assert.That( File.Exists( ReportService.THRESHOLD_FILE ));
        }


        [ Test, Category( "GetThreshold" ) ]
        public void Getthresholds_ThresholdHasChanged_SerializesThreshold ()
        {
            var operation = new Operation() { Id = 1 };

            using ( var service = GetReportService( false ) ) {

                var thresholds = service.GetThresholds();
                thresholds[ operation ] = 666;
            }

            using ( var service = GetReportService( false ) ) {
                
                var thresholds = service.GetThresholds();
                Assert.That( 666 == thresholds[ operation ], $"thresholds[ operation ] == { thresholds[ operation ] }");
            }
        }


        #region Factory


        private DbConnection _connection;

        private ReportService GetReportService ( bool fillEmployees = true )
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();

            var options = new DbContextOptionsBuilder< WorkSpeedDbContext >().UseSqlite( _connection ).Options;

                using (var dbContext = new WorkSpeedDbContext(options) ) { 

                    dbContext.Database.EnsureCreated();

                    if ( fillEmployees ) { 

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
                    }
                };
            var stub = new Mock< IProductivityBuilder >();

            var reportService = new ReportService( new WorkSpeedDbContext(options), stub.Object );

            return reportService;
        }

        #endregion
    }
}
