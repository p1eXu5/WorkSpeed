using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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



        [ Test ]
        public void ImportService_WhenCreating_DoesNotSeedProducts ()
        {
            // Arrange:
            // Action:
            var service = GetImportService();

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 0 );
        }

        [ Test ]
        public void ImportFromXlsx_CanAddProduct ()
        {
            // Arrange:
            var service = GetImportService();
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product " } };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts.Length == 1 );
        }

        [ Test ]
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

        [ Test ]
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

        [ Test ]
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

        [ Test ]
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

        [ Test ]
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

        [ Test ]
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



        [ Test ]
        public void ImportService_WhenCreating_DoesNotSeedEmployees ()
        {
            // Arrange:
            // Action:
            var service = GetImportService();

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 0 );
        }

        [ Test ]
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

        [ Test ]
        public void ImportFromXlsx_EmployeeExistInDb_DoesNotAddEmployee ()
        {
            // Arrange:
            var service = GetImportService();
            service.Products = new[] { new Product { Id = 01223456, Name = "Test Product" } };

            // Action:
            service.ImportFromXlsx( PRODUCTS, null );
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbEmployees = service.DbContext.Employees.ToArray();
            Assert.That( dbEmployees.Length == 1 );
        }

        #region Factory

        private const string PRODUCTS = "Products";
        private const string EMPLOYEES = "Employees";
        private const string PRODUCTIVITY = "Productivity";

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

        /// <summary>
        /// Fake ImportService
        /// </summary>
        private class ImportServiceFake : ImportService
        {
            public ImportServiceFake ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext, typeRepository ) { }

            public IEnumerable< Product > Products { get; set; }
            public IEnumerable< Employee > Employees { get; set; }
            public IEnumerable< AllActions > Productivity { get; set; }

            protected override IEnumerable< IEntity > GetDataFromFile ( string fileName )
            {
                switch ( fileName ) {
                    case PRODUCTS:
                        return Products;
                    case EMPLOYEES:
                        return Employees;
                    case PRODUCTIVITY:
                        return Productivity;
                    default:
                        return new IEntity[0];
                }
            }
        }

        #endregion
    }

}
