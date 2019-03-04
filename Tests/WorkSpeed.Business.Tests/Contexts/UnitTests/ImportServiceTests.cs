using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using Agbm.NpoiExcel;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WorkSpeed.Business.Contexts;
using WorkSpeed.Data.Context;
using WorkSpeed.Data.Models;
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
            var service = GetFakeImportService();

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 0 );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ValidProduct_CanAddProduct ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product " } };

            // Action:
            service.ImportFromXlsx( PRODUCTS );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts, Is.Not.Empty );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_TwoIdenticalProduct_AddsOneProduct ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] {
                new Product { Id = 01223456, Name = "Test Product" },
                new Product { Id = 01223456, Name = "Test Product" }
            };

            // Action:
            service.ImportFromXlsx( PRODUCTS );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_TwoProductWithSameId_SecondWithMeasure_AddsProductWithMeasure ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] {
                new Product { Id = 01223456, Name = "Test Product" },
                new Product { Id = 01223456, Name = "Test Product", ItemWeight = 1.5f }
            };

            // Action:
            service.ImportFromXlsx( PRODUCTS );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray()[0];
            Assert.That( dbProducts.ItemWeight, Is.EqualTo( 1.5f ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_TwoProductWithSameId_FirstWithMeasure_AddsProductWithMeasure ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] {
                new Product { Id = 01223456, Name = "Test Product", ItemWeight = 1.5f },
                new Product { Id = 01223456, Name = "Test Product" }
            };

            // Action:
            service.ImportFromXlsx( PRODUCTS );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray()[0];
            Assert.That( dbProducts.ItemWeight, Is.EqualTo( 1.5f ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_DoesNotAddProduct ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( PRODUCTS );
            service.ImportFromXlsx( PRODUCTS );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 1 );
        }

        [ TestCase( 0 ), TestCase( -1 ), Category( "Product" ) ] 
        public void ImportFromXlsx_ProductHasWrongId_DoesNotAddProduct ( int id )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] { new Product { Id = id, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts, Is.Empty );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductsWithSameId_AddsFirstProduct ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Products = new[] {
                new Product { Id = 01223456, Name = "Test Product" },
                new Product { Id = 01223456, Name = "Test Product2" }
            };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length, Is.EqualTo( 1 ) );
            Assert.That( dbProducts[0].Name, Is.EqualTo( "Test Product" ) );
        }

        [ Test, Category( "Product" ) ]
        public void ImportFromXlsx_ProductExistInDb_AddingProductHasDifferentName_DoesNotChangeDbProductName ()
        {
            // Arrange:
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 0 );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_ValidEmployee_CanAddEmployee ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 1 );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_TwoIdenticalId_AddsOneEmployee ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee" },
                new Employee { Id = "AR23456", Name = "Test Employee" }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_TwoIdenticalId_FirstWithAppointment_AddsEmployeeWithAppointment ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "кл."} },
                new Employee { Id = "AR23456", Name = "Test Employee" }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray()[0];
            Assert.That( dbEmployees.Appointment, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_TwoIdenticalId_SecondWithAppointment_AddsEmployeeWithAppointment ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee" },
                new Employee { Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "кл."} }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray()[0];
            Assert.That( dbEmployees.Appointment, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_ExistInDb_DoesNotAddEmployee ()
        {
            // Arrange:
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
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
            var service = GetFakeImportService();
            service.Employees = new[] { new Employee { Id = "AR12345", Name = "" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees, Is.Empty );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeesWithSameId_AddsFirstEmployees()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee" },
                new Employee { Id = "AR23456", Name = "Test Employee2" }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 1 );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasUnknownRank_AddsWithDefaultRank ()
        {
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Rank = new Rank { Number = 827 } }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Rank.Number, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasKnownRank_AddsWithRank ()
        {
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Rank = new Rank { Number = 3 } }
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
            var service = GetFakeImportService();

            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" } };
            service.ImportFromXlsx( EMPLOYEES, null );

            service.Employees[0] = new Employee{ Id = "AR23456", Name = "Test Employee", Rank = new Rank { Number = 3} };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Rank, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasUnknownAppointment_AddsWithDefaultAppointment ()
        {
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "клк" } }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Appointment.Id, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasKnownAppointment_AddsWithAppointment ()
        {
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "кл" } }
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
            var service = GetFakeImportService();

            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" } };
            service.ImportFromXlsx( EMPLOYEES, null );

            service.Employees[0] = new Employee{ Id = "AR23456", Name = "Test Employee", Appointment = new Appointment { Abbreviations = "кл" } };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Appointment, Is.Not.Null );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasUnknownPosition_AddsWithDefaultPosition ()
        {
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Position = new Position { Abbreviations = "крупнъяг" } }
            };

            // Action:
            service.ImportFromXlsx( EMPLOYEES, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees[0].Position.Id, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "Employee" ) ]
        public void ImportFromXlsx_EmployeeHasKnownPosition_AddsWithPosition ()
        {
            var service = GetFakeImportService();
            service.Employees = new[] {
                new Employee { Id = "AR23456", Name = "Test Employee", Position = new Position { Abbreviations = "мез3" } }
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
            var service = GetFakeImportService();

            service.Employees = new[] { new Employee { Id = "AR23456", Name = "Test Employee" } };
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

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportService_WhenCreating_DoesNotSeedDoubleAddressActions ()
        {
            // Arrange:
            // Action:
            var service = GetFakeImportService();

            // Assert:
            var dbEmployees = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( dbEmployees.Length == 0 );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_ValidDoubleAddressAction_CanAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new EmployeeActionBase[] {
                GetDoubleAddressAction()
            };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Not.Empty );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx__OneDoubleAddressAction_SeveralDetails__AddsOneActionAndSeveralDetails ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new EmployeeActionBase[] {
                GetDoubleAddressAction(),
                GetDoubleAddressAction(),
                GetDoubleAddressAction(),
            };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions.Length, Is.EqualTo( 1 ) );
            Assert.That( actions[0].DoubleAddressDetails.Count, Is.EqualTo( 3 ) );
        }

        [ TestCase( "ZP8-12345" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_IdLengthLess10_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "ZP8-12345T5" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_IdContainsLetterInSuffix_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }
        
        [ TestCase( "3P8-123456" ), TestCase( "Z38-123456" ), TestCase( "238-123456" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_IdFirstTwoNotLetters_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "ZPD-123456" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_IdThirdNotDigit_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "ZP8+123456" ), TestCase( "ZP8 123456" ) ]
        [ TestCase( "ZP8Z123456" ), TestCase( "ZP87123456" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_IdFourthNotDash_DoesNotAddDoubleAddressAction ( string id )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Id = id;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "1997, 01, 01" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_StartTimeLess1998_01_01_DoesNotAddDoubleAddressAction ( string date )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].StartTime = DateTime.Parse( date );

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ TestCase( "3019, 02, 10" ), Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_StartTimeIsFuture_DoesNotAddDoubleAddressAction ( string date )
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].StartTime = DateTime.Parse( date );

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_DurationIsZero_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Duration = TimeSpan.Zero;

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_WrongEmployeeId_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Employee.Id = "AR-1234";

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_OperationDoesNotExist_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction()
            };
            service.Actions[0].Operation.Name = "";

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_ProductDoesNotExist_AddsProduct ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetDoubleAddressAction(),
                GetDoubleAddressAction()
            };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var products = service.DbContext.Products.ToArray();
            Assert.That( products, Is.Not.Empty );
            Assert.That( products.Length, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_DoubleAddressActionExistInDb_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] { 
                GetDoubleAddressAction()
            };
            service.ImportFromXlsx( ACTIONS, null );

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions.Length, Is.EqualTo( 1 ) );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_DifferentEmployees_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] { 
                GetDoubleAddressAction(),
                GetDoubleAddressAction()
            };
            service.Actions[1].Employee = new Employee { Id = "ZZ12345", Name = "Other Employee" };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        [ Test, Category( "DoubleAddressAction" ) ]
        public void ImportFromXlsx_DifferentOperations_DoesNotAddDoubleAddressAction ()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] { 
                GetDoubleAddressAction(),
                GetDoubleAddressAction()
            };
            service.Actions[1].Operation = new Operation { Name = "Упаковка товара в места" };

            // Action:
            service.ImportFromXlsx( ACTIONS, null );

            // Assert:
            var actions = service.DbContext.DoubleAddressActions.ToArray();
            Assert.That( actions, Is.Empty );
        }

        #endregion


        [Test, Category("ReceptionAction")]
        public void ImportFromXlsx_ValidReceptionAction_CanAddReceptionAction()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetReceptionAction()
            };

            // Action:
            service.ImportFromXlsx(ACTIONS, null);

            // Assert:
            var actions = service.DbContext.ReceptionActions.ToArray();
            Assert.That(actions, Is.Not.Empty);
        }


        [Test, Category("InventoryAction")]
        public void ImportFromXlsx_ValidInventoryAction_CanAddInventoryAction()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new[] {
                GetInventoryAction()
            };

            // Action:
            service.ImportFromXlsx(ACTIONS, null);

            // Assert:
            var actions = service.DbContext.InventoryActions.ToArray();
            Assert.That(actions, Is.Not.Empty);
        }


        [Test, Category("ShipmentAction")]
        public void ImportFromXlsx_ValidShipmentAction_CanAddShipmentAction()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new EmployeeActionBase[] {
                GetShipmentAction(),
                GetShipmentAction()
            };
            service.Actions[1].Employee = new Employee { Id = "ZZ12345", Name = "OtherEmployee" };

            // Action:
            service.ImportFromXlsx(ACTIONS, null);

            // Assert:
            var actions = service.DbContext.ShipmentActions.ToArray();
            Assert.That(actions, Is.Not.Empty);
        }


        [Test, Category("OtherAction")]
        public void ImportFromXlsx_ValidOtherAction_CanAddOtherAction()
        {
            // Arrange:
            var service = GetFakeImportService();
            service.Actions = new EmployeeActionBase[] {
                GetOtherAction()
            };

            // Action:
            service.ImportFromXlsx(ACTIONS, null);

            // Assert:
            var actions = service.DbContext.OtherActions.ToArray();
            Assert.That(actions, Is.Not.Empty);
        }



        #region Factory

        private const string PRODUCTS = "Products";
        private const string EMPLOYEES = "Employees";
        private const string ACTIONS = "Actions";

        private DbConnection _connection;

        private FakeImportService GetFakeImportService ()
        {
            _connection = new SqliteConnection( "DataSource=:memory:" );
            _connection.Open();

            var options = new DbContextOptionsBuilder< WorkSpeedDbContext >().UseSqlite( _connection ).Options;
            var dbContext = new WorkSpeedDbContext( options );
            dbContext.Database.EnsureCreated();

            var stubITypeRepository = new Mock< ITypeRepository >();

            var importServise = new FakeImportService( dbContext, stubITypeRepository.Object );
            return importServise;
        }

        private DoubleAddressAction GetDoubleAddressAction ()
        {
            return new DoubleAddressAction { 
                        Id = "ZP8-123456",
                        StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                        Duration = TimeSpan.FromSeconds( 34 ),
                        DocumentName = "",
                        Employee = new Employee { Id = "AR12345", Name = "Anton" },
                        Operation = new Operation { Name = "Подбор товара" },
                        DoubleAddressDetails = new List< DoubleAddressActionDetail >{ new DoubleAddressActionDetail {
                            Product = new Product { Id = 0006634, Name = "Test Product" },
                            ProductQuantity = 1,
                            SenderAddress = new Address { Letter = "К", Row = 1, Section = 1, Shelf = 1, Box = 1 },
                            ReceiverAddress = new Address { Letter = "У", Row = 1, Section = 1, Shelf = 1, Box = 1 }
                        }
                    }
            };
        }

        private ReceptionAction GetReceptionAction ()
        {
            return new ReceptionAction { 
                        Id = "ZP8-123456",
                        StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                        Duration = TimeSpan.FromSeconds( 34 ),
                        DocumentName = "",
                        Employee = new Employee { Id = "AR12345", Name = "Anton" },
                        Operation = new Operation { Name = "Сканирование товара" },

                        ReceptionActionDetails = new List< ReceptionActionDetail >{ new ReceptionActionDetail {
                            Product = new Product { Id = 0006634, Name = "Test Product" },
                            ProductQuantity = 1,
                            ScanQuantity    = 3,
                            Address = new Address { Letter = "К", Row = 1, Section = 1, Shelf = 1, Box = 1 }
                        }
                    }
            };
        }

        private InventoryAction GetInventoryAction ()
        {
            return new InventoryAction { 
                        Id = "ZP8-123456",
                        StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                        Duration = TimeSpan.FromSeconds( 34 ),
                        DocumentName = "",
                        Employee = new Employee { Id = "AR12345", Name = "Anton" },
                        Operation = new Operation { Name = "Инвентаризация" },

                        InventoryActionDetails = new List< InventoryActionDetail >{ new InventoryActionDetail {
                            Product = new Product { Id = 0006634, Name = "Test Product" },
                            ProductQuantity = 1,
                            AccountingQuantity = 1,
                            Address = new Address { Letter = "К", Row = 1, Section = 1, Shelf = 1, Box = 1 }
                        }
                    }
            };
        }

        private ShipmentAction GetShipmentAction ( string employeeId = null, string employeeName = null )
        {
            return new ShipmentAction { 
                        Id = "ZP8-123456",
                        StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                        Duration = TimeSpan.FromSeconds( 34 ),
                        DocumentName = "",
                        Employee = new Employee { Id = employeeId ?? "AR12345", Name = employeeName ?? "Anton" },
                        Operation = new Operation { Name = "Выгрузка машины" },

                        Weight = 1.0f,
                        Volume = 1.0f,
                        ClientCargoQuantity = 1.0f,
                        CommonCargoQuantity = 1.0f
            };
        }

        private OtherAction GetOtherAction ()
        {
            return new OtherAction { 
                        Id = "ZP8-123456",
                        StartTime = DateTime.Parse( "12.12.2017 12:23:43", CultureInfo.CurrentCulture ),
                        Duration = TimeSpan.FromSeconds( 34 ),
                        DocumentName = "",
                        Employee = new Employee { Id = "AR12345", Name = "Anton" },
                        Operation = new Operation { Name = "Прочие операции" }
            };
        }

        /// <summary>
        /// Fake ImportService
        /// </summary>
        private class FakeImportService : ImportService
        {
            public FakeImportService ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext, typeRepository ) { }

            public Product[] Products { get; set; }
            public Employee[] Employees { get; set; }
            public EmployeeActionBase[] Actions { get; set; }

            protected override IEnumerable< IEntity > GetDataFromFile ( string fileName )
            {
                switch ( fileName ) {
                    case PRODUCTS:
                        return Products;
                    case EMPLOYEES:
                        return Employees;
                    case ACTIONS:
                        return Actions;
                    default:
                        return new IEntity[0];
                }
            }
        }

        #endregion
    }

}
