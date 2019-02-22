using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Context.ReportService;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Data.Tests.Context.ReportService.UnitTests
{
    [ TestFixture ]
    public class WorkSpeedDbContextExtensionsTests
    {
        [ SetUp ]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }

        [ TearDown ]
        public void CloseConnection ()
        {
            _connection.Close();
            _options = null;
        }

        [ Test ]
        public void GetEmployeeActions_ReturnsActionsSortedByDescendingByStartTime ()
        {
            var employee = new Employee { Id = "AR00001", Name = "Employee 1", IsActive = true };

            using ( var dbContext = GetContext() ) {

                dbContext.Database.EnsureCreated();

                dbContext.Employees.Add( employee );
                dbContext.SaveChanges();

                dbContext.DoubleAddressActions.AddRange( new DoubleAddressAction[] {
                    new DoubleAddressAction { 
                        Id = "1", 
                        StartTime = DateTime.Parse( "22.02.2019 8:00:45" ),
                        Duration = TimeSpan.FromSeconds( 20 ),
                        Employee = employee,
                        Operation = dbContext.Operations.First( o => o.Id == 8 )
                    }, 
                });

                dbContext.InventoryActions.AddRange( new InventoryAction[] {
                    new InventoryAction { 
                        Id = "1", 
                        StartTime = DateTime.Parse( "22.02.2019 9:00:45" ),
                        Duration = TimeSpan.FromSeconds( 20 ),
                        Employee = employee,
                        Operation = dbContext.Operations.First( o => o.Id == 8 )
                    }, 
                });

                dbContext.ShipmentActions.AddRange( new ShipmentAction[] {
                    new ShipmentAction { 
                        Id = "1", 
                        StartTime = DateTime.Parse( "22.02.2019 19:00:45" ),
                        Duration = TimeSpan.FromSeconds( 20 ),
                        Employee = employee,
                        Operation = dbContext.Operations.First( o => o.Id == 8 )
                    },  
                });

                dbContext.SaveChanges();
            }

            using ( var dbContext = GetContext() ) {

                var actions = dbContext.GetEmployeeActions( DateTime.Parse( "22.02.2019 8:00:00" ), DateTime.Parse( "23.02.2019 8:00:00" ) );

                foreach ( var action in actions ) {
                    
                    Assert.That( action.Key.Id, Is.EqualTo( employee.Id ) );
                    Assert.That( action.Skip( 0 ).First().StartTime, Is.EqualTo( DateTime.Parse( "22.02.2019 19:00:45" ) ) );
                    Assert.That( action.Skip( 1 ).First().StartTime, Is.EqualTo( DateTime.Parse( "22.02.2019 9:00:45" ) ) );
                    Assert.That( action.Skip( 2 ).First().StartTime, Is.EqualTo( DateTime.Parse( "22.02.2019 8:00:45" ) ) );
                }
            }

        }


        #region Factory

        private DbConnection _connection;
        private DbContextOptions< WorkSpeedDbContext > _options;

        private WorkSpeedDbContext GetContext ()
        {
            if ( _options != null ) return new WorkSpeedDbContext( _options );

            _connection = new SqliteConnection( "DataSource = :memory:" );
            _connection.Open();

            _options = new DbContextOptionsBuilder< WorkSpeedDbContext >().UseSqlite( _connection ).Options;

            return new WorkSpeedDbContext( _options );
        }

        #endregion
    }
}
