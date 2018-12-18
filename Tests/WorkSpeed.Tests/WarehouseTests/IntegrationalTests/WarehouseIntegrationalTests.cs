using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using NpoiExcel;
using NUnit.Framework;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Tests.WarehouseTests.IntegrationalTests
{
    [TestFixture]
    public class WorkSpeedIntegrationalTests
    {
        [ SetUp ]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }

        [ Test ]
        public void HasProducts_ContextDoesNotHaveProducts_ReturnsFalse ()
        {
            // Arrange:
            var businessContext = GetBusinessContext();
            var dataImporter = GetDataImporter();
            var warehouse = GetWarehouse( businessContext, dataImporter );

            // Action:
            Task<bool> res = warehouse.HasProductsAsync();
            res.Wait();

            // Assert:
            Assert.That( false == res.Result );
        }

        [ Test ]
        public void ImportWithProduct_FileWithProducts_AddsProductsToBusinessContext()
        {
            // Arrange:
            var businessContext = GetBusinessContext();
            var dataImporter = GetDataImporter();
            var warehouse = GetWarehouse( businessContext, dataImporter );

            var file = "products.xlsx".AppendAssemblyPath( "WarehouseTests\\TestFiles" );

            // Action:
            Task<bool> res = warehouse.ImportAsync< ProductImportModel >( file );
            res.Wait();

            // Assert:
            Assert.That( businessContext.GetProducts().Any );
        }

        [ Test ]
        public void ImportWithProduct_FileWithGatheringActions_AddsGatheringActionsToBusinessContext()
        {
            // Arrange:
            var businessContext = GetBusinessContext();
            var dataImporter = GetDataImporter();
            var warehouse = GetWarehouse( businessContext, dataImporter );

            var file = "gathering.xls".AppendAssemblyPath( "WarehouseTests\\TestFiles" );

            // Action:
            Task<bool> res = warehouse.ImportAsync< GatheringImportModel >( file );
            res.Wait();

            // Assert:
            Assert.That( businessContext.GatheringActions.Any() );
        }


        #region Factory

        private IWorkSpeedBusinessContext GetBusinessContext()
        {
            return new RuntimeWorkSpeedBusinessContext();
        }

        private IDataImporter GetDataImporter()
        {
            return new ExcelDataImporter();
        }

        private Warehouse GetWarehouse( IWorkSpeedBusinessContext context, IDataImporter importer )
        {
            return new Warehouse( context, importer );
        }

        #endregion
    }
}
