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
            service.Products = new[] { new Product { Id = 01223456, Name = originName, ItemLength = } };
            service.ImportFromXlsx( PRODUCTS, null );

            // Assert:
            var dbProducts = service.DbContext.Products.ToArray();
            Assert.That( dbProducts[0].Name, Is.EqualTo( originName ) );
        }

        #region Factory

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

        private const string PRODUCTS = "Products";

        private class ImportServiceFake : ImportService
        {
            public ImportServiceFake ( WorkSpeedDbContext dbContext, ITypeRepository typeRepository ) : base( dbContext, typeRepository ) { }

            public IEnumerable< Product > Products { get; set; }

            protected override bool TryGetData ( string fileName, out IEnumerable< IEntity > data )
            {
                switch ( fileName ) {
                    case PRODUCTS:
                        data = Products;
                        return true;
                    default:
                        data = new IEntity[0];
                        return false;
                }
            }
        }

        #endregion
    }

}
