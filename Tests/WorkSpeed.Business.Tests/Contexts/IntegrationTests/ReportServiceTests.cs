using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using Agbm.Helpers.Extensions;
using Agbm.NpoiExcel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Contexts.Productivity;
using WorkSpeed.Business.Contexts.Productivity.Models;
using WorkSpeed.Business.FileModels;
using WorkSpeed.Business.FileModels.Converters;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Tests.Contexts.IntegrationTests
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


        [ Test, Category( "BuildProductivities" ) ]
        public void BuildProductivities_IntegrationTest ()
        {
            var service = GetFakeRaportService();

            var sheetTable = ExcelImporter.GetSheetTable( "test.xlsx".AppendAssemblyPath("Contexts\\IntegrationTests") );
            TypeRepository.TryGetPropertyMap( sheetTable, typeof( TestImportModel ), out var propertyMap );
            var visitor = new TestImportModelVisitor( _operations );
            var actions = ExcelImporter.GetDataFromTable( sheetTable, propertyMap, new ImportModelConverter< TestImportModel, EmployeeActionBase >( visitor ) );

            var shortBreaks = new ShortBreakSchedule {
                Duration = TimeSpan.FromMinutes( 10 ),
                FirstBreakTime = new TimeSpan( 9, 55, 0),
                Periodicity = TimeSpan.FromHours( 2 )
            };

            var shift = new Shift {
                Lunch = TimeSpan.FromMinutes( 30 ),
            };

            var productivities = service.Build( actions, shortBreaks, shift );

            var employeeProductivity = new EmployeeProductivity( new Employee(), productivities );

            var totalTime = employeeProductivity.GetTotalWorkHours();
            Assert.That( totalTime, Is.GreaterThan( 5.7 ) );

            var pause = employeeProductivity.DowntimePeriods.Sum( d => d.Duration.TotalSeconds );
            Assert.That( pause, Is.GreaterThan( 16000 ) );
        }


        #region Factory

        private Operation[] _operations;
        private DbConnection _connection;

        private FakeReportService GetFakeRaportService ( )
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();

            var options = new DbContextOptionsBuilder< WorkSpeedDbContext >().UseSqlite( _connection ).Options;

            using (var dbContext = new WorkSpeedDbContext(options) ) { 

                dbContext.Database.EnsureCreated();
            }


            var reportService = new FakeReportService( new WorkSpeedDbContext(options), new ProductivityBuilder() );
            reportService.ReloadAllCollections();
            _operations = reportService.OperationCollection.ToArray();

            return reportService;
        }




        public class TestImportModelVisitor : IImportModelVisitor
        {
            private readonly Operation[] _operations;

            public TestImportModelVisitor ( Operation[] operations )
            {
                _operations = operations;
            }

            public IEntity GetDbModel ( ImportModel importModel )
            {
                if ( !(importModel is TestImportModel im) ) throw new ArgumentException();

                if ( im.Operation == "Подбор товара" ) {
                    return new DoubleAddressAction {
                        StartTime = im.StartTime,
                        Duration = TimeSpan.FromSeconds( im.Duration ),
                        Operation = _operations.First( o => o.Name.Equals( "Подбор товара" ) ),
                    };
                }

                if ( im.Operation == "Упаковка товара в места" ) {
                    return new DoubleAddressAction {
                        StartTime = im.StartTime,
                        Duration = TimeSpan.FromSeconds( im.Duration ),
                        Operation = _operations.First( o => o.Name.Equals( "Упаковка товара в места" ) ),
                        DoubleAddressDetails = new List< DoubleAddressActionDetail > {
                            new DoubleAddressActionDetail { ProductQuantity = im.Quantity }
                        }
                    };
                }

                if ( im.Operation == "Перемещение товара" ) {
                    return new DoubleAddressAction {
                        StartTime = im.StartTime,
                        Duration = TimeSpan.FromSeconds( im.Duration ),
                        Operation = _operations.First( o => o.Name.Equals( "Перемещение товара" ) ),
                    };
                }

                if ( im.Operation == "Погрузка машины" ) {
                    return new ShipmentAction {
                        StartTime = im.StartTime,
                        Duration = TimeSpan.FromSeconds( im.Duration ),
                        Operation = _operations.First( o => o.Name.Equals( "Погрузка машины" ) ),
                    };
                }

                if ( im.Operation == "Выгрузка машины" ) {
                    return new ShipmentAction {
                        StartTime = im.StartTime,
                        Duration = TimeSpan.FromSeconds( im.Duration ),
                        Operation = _operations.First( o => o.Name.Equals( "Выгрузка машины" ) ),
                    };
                }

                return null;
            }
        }


        public class FakeReportService : ReportService
        {
            public FakeReportService ( WorkSpeedDbContext dbContext, IProductivityBuilder builder ) : base( dbContext, builder ) { }

            public (IReadOnlyDictionary< Operation, IProductivity >, HashSet< Period >) Build ( IEnumerable< EmployeeActionBase > actions, ShortBreakSchedule breaks, Shift shift )
            {
                return BuildProductivities( actions, breaks, shift );
            }
        }

        #endregion
    }
}
