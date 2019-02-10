using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Business.Contexts.Contracts;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.Models;
using NUnit.Framework;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.Tests.Contexts.UnitTests
{
    [ TestFixture ]
    public class ImportServiceTests
    {
        [ TearDown ]
        public void CloseConnection ()
        {
            _connection?.Close();
        }


        #region Product

        [ Test, Category( "Product" ) ]
        public void ImportService_WhenCreating_DoesNotSeedProducts ()
        {
            // Arrange:
            // Action:
            var service = GetImportService();

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 0 );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_CanAddProduct ()
        {
            // Arrange:
            var service = GetImportService();
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product " } };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts, Is.Not.Empty );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_DoesNotAddProduct ()
        {
            // Arrange:
            var service = GetImportService();
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 1 );
        }

        [ TestCase( 0 ), TestCase( -1 ), Category( "Product" ) ] 
        public void ImportFromXlsx_ProductHasWrongId_DoesNotAddProduct ( int id )
        {
            // Arrange:
            var service = GetImportService();
            service.Products = new[] { new Product { Id = id, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts, Is.Empty );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ByDefault_CannotAddProductWithTheSameId ()
        {
            // Arrange:
            var service = GetImportService();
            service.Products = new[] {
                new Product { Id = 01223456, Name = "Test Product" },
                new Product { Id = 01223456, Name = "Test Product2" },
            };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 1 );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_AddingProductHasDifferentName_DoesNotChangeDbProductName ()
        {
            // Arrange:
            var service = GetImportService();
            var originName = "TestProduct";
            service.Products = new[] { new Product { Id = 01223456, Name = originName } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Action:
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product2" } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts[0].Name, Is.EqualTo( originName ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_AddingProductHasDifferentItemLength_UpdateDbProductItemLength ()
        {
            // Arrange:
            var service = GetImportService();
            var originName = "TestProduct";
            service.Products = new[] { new Product { Id = 01223456, Name = originName } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Action:
            service.Products = new[] { new Product { Id = 01223456, Name = originName, ItemLength = 15.0f } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts[0].ItemLength, Is.EqualTo( 15.0f ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_AddingProductHasDifferentItemWidth_UpdateDbProductItemWidth ()
        {
            // Arrange:
            var service = GetImportService();
            var originName = "TestProduct";
            service.Products = new[] { new Product { Id = 01223456, Name = originName } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Action:
            service.Products = new[] { new Product { Id = 01223456, Name = originName, ItemWidth = 15.0f } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts[0].ItemWidth, Is.EqualTo( 15.0f ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_AddingProductHasDifferentItemHeight_UpdateDbProductItemHeight ()
        {
            // Arrange:
            var service = GetImportService();
            var originName = "TestProduct";
            service.Products = new[] { new Product { Id = 01223456, Name = originName } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Action:
            service.Products = new[] { new Product { Id = 01223456, Name = originName, ItemHeight = 15.0f } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts[0].ItemHeight, Is.EqualTo( 15.0f ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_AddingProductHasDifferentItemWeight_UpdateDbProductItemWeight ()
        {
            // Arrange:
            var service = GetImportService();
            var originName = "TestProduct";
            service.Products = new[] { new Product { Id = 01223456, Name = originName } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Action:
            service.Products = new[] { new Product { Id = 01223456, Name = originName, ItemWeight = 15.0f } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts[0].ItemWeight, Is.EqualTo( 15.0f ) );
        }

        #endregion


        #region Employee

        [ Test, Category( "Employee" ) ]
        public void ImportService_WhenCreating_DoesNotSeedEmployees ()
        {
            // Arrange:
            // Action:
            var service = GetImportService();

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 0 );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_CanAddEmployee ()
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 1 );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_ExistInDb_DoesNotAddEmployee ()
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 1 );
        }

        [ TestCase("A928374"), TestCase( "1234567" ), TestCase( "1T34567" ), Category( "Employee" ) ] 
        public void ImportFromXlsx_IdFistNot2Letters_DoesNotAddEmployee ( string id )
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] { new Employee { Id = id, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees, Is.Empty );
        }

        [TestCase("042"), TestCase("AR9283746"), Category( "Employee" ) ] 
        public void ImportFromXlsx_IdLengthNot7_DoesNotAddEmployee ( string id )
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] { new Employee { Id = id, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees, Is.Empty );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_IdLastFiveNotDigits_DoesNotAddEmployee ()
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] { new Employee { Id = "AR-1234", Name = "Test Employee" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees, Is.Empty);
        }

        [ Test, Category( "Employee" ) ] 
        public void ImportFromXlsx_HasNameLengthLess1_DoesNotAddEmployee ()
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] { new Employee { Id = "AR12345", Name = "" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees, Is.Empty );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_ByDefault_CannotAddEmployeesWithTheSameId()
        {
            // Arrange:
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee" },
                new Employee { Id = "AR23456", Name = "Test Employee2" },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 1 );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasUnknownRank_AddsWithNullRank ()
        {
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Rank = new Rank { Number = 827 } },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Rank, Is.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasKnownRank_AddsWithRank ()
        {
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Rank = new Rank { Number = 3 } },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Rank, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeWithRank_DbEmployeeWithNullRank_UpdatesDbEmployeeRank ()
        {
            var service = GetImportService();

            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" }, };
            service.ImportFromXlsx( EMPLOYEES, null );

            service.Employees[0] = new Employee{ Id = "AR23456", Name = "Test Employee", Rank = new Rank { Number = 3} };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Rank, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasUnknownAppointment_AddsWithNullAppointment ()
        {
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "клк" } },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Appointment, Is.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasKnownAppointment_AddsWithAppointment ()
        {
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "кл" } },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Appointment, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeWithAppointment_DbEmployeeWithNullAppointment_UpdatesDbEmployeeAppointment ()
        {
            var service = GetImportService();

            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" }, };
            service.ImportFromXlsx( EMPLOYEES, null );

            service.Employees[0] = new Employee{ Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "кл" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Appointment, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasUnknownPosition_AddsWithNullPosition ()
        {
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Position = new Position { Abbreviations = "крупнъяг" } },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Position, Is.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasKnownPosition_AddsWithPosition ()
        {
            var service = GetImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Position = new Position { Abbreviations = "мез3" } },
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Position, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeWithPosition_DbEmployeeWithNullPosition_UpdatesDbEmployeePosition ()
        {
            var service = GetImportService();

            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" }, };
            service.ImportFromXlsx( EMPLOYEES, null );

            service.Employees[0] = new Employee{ Id = "AR23456", Name = "Test Employee", Position = new Position { Abbreviations = "мез3" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Position, Is.Not.Null );
        }

        #endregion


        #region DoubleAddressAction

        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportService_WhenCreating_DoesNotSeedDoubleAddressActions ()
        {
            // Arrange:
            // Action:
            var service = GetImportService();

            // Assert:
            var dbEmployees = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( dbEmployees.Length == 0 );
        }

        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_CanAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Not.Empty );
        }

        [ TestCase( "ZP8-123450" ), TestCase( "ZP8-1234" ), Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_IdLengthNot9_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "3P8-12345" ), TestCase( "Z38-12345" ), TestCase( "238-12345" ), Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_IdFirstTwoNotLetters_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "ZPD-12345" ), Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_IdThirdNotDigit_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "ZP8+12345" ), TestCase( "ZP8 12345" ) ]
        [ TestCase( "ZP8Z12345" ), TestCase( "ZP8712345" ), Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_IdFourthNotDash_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "1997, 01, 01" ), Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_StartTimeLess1998_01_01_DoesNotAddDoubleAddressAction ( string date )
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.StartTime = DateTime.Parse( date );

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "3019, 02, 10" ), Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_StartTimeIsFuture_DoesNotAddDoubleAddressAction ( string date )
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.StartTime = DateTime.Parse( date );

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_DurationIsZero_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Duration = TimeSpan.Zero;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_WrongEmployeeId_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Employee.Id = "AR-1234";

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_OperationDoesNotExist_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.Operation.Name = "";

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_ProductDoesNotExist_AddsProduct ()
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] {
                new AllActions { DoubleAddressAction = GetValidDoubleAddressAction() }
            };
            service.AllActions[0].DoubleAddressAction.DoubleAddressDetails[0].ProductId = 1;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }





        [ Test, Category( "DoubleAddressActions" ) ]
        public void ImportFromXlsx_DoubleAddressActionExistInDb_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetImportService();
            service.AllActions = new[] { new AllActions {
                DoubleAddressAction = new DoubleAddressAction {
                    Id = "ZP8-000498",
                    StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                    Duration = TimeSpan.FromSeconds( 34 ),
                    DocumentName = "",
                    Employee = new Employee { Id = "AR32187", Name = "Anton" },
                    Operation = new Operation { Name = "Подбор товара" },
                    DoubleAddressDetails = new List< DoubleAddressActionDetail >(),
                },
            } };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        #endregion

        #region Factory

        private const string PRODUCTS = "Products";
        private const string EMPLOYEES = "Employees";
        private const string ACTIONS = "AllActions";

        private DbConnection _connection;

        private ImportServiceFake GetImportService ()
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();

            var options = new DbContextOptionsBuilder< WorkSpeedDbContext >().UseSqlite( _connection ).Options;
            var dbContext = new WorkSpeedDbContext( options );
            dbContext.Database.EnsureCreated();

            var stubITypeRepository = new Mock< ITypeRepository >();

            var importServise = new ImportServiceFake( dbContext, stubITypeRepository.Object );
            return importServise;
        }

        private DoubleAddressAction GetValidDoubleAddressAction ()
        {
            return new DoubleAddressAction { 
                        Id = "ZP8-000498",
                        StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                        Duration = TimeSpan.FromSeconds( 34 ),
                        DocumentName = "",
                        Employee = new Employee { Id = "AR12345", Name = "Anton" },
                        Operation = new Operation { Name = "Подбор товара" },
                        DoubleAddressDetails = new List< DoubleAddressActionDetail >{ new DoubleAddressActionDetail {
                            Product = new Product { Id = 0006634, Name = "Test Product", },
                            ProductQuantity = 1,
                        },
                    }
            };
        }

        /// <summary>
        /// Fake ImportService
        /// </summary>
        private class ImportServiceFake : ImportService
        {
            public ImportServiceFake ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext, typeRepository ) { }

            public Product[] Products { get; set; }
            public Employee[] Employees { get; set; }
            public AllActions[] AllActions { get; set; }

            protected override IEnumerable< IEntity > GetDataFromFile ( string fileName )
            {
                switch ( fileName ) {
                    case PRODUCTS:
                        return Products;
                    case EMPLOYEES:
                        return Employees;
                    case ACTIONS:
                        return AllActions;
                    default:
                        return new IEntity[0];
                }
            }
        }

        #endregion
    }

}
