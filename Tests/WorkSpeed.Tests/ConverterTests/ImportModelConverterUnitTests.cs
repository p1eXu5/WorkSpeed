using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NpoiExcel;
using NUnit.Framework;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

namespace WorkSpeed.Tests.ConverterTests
{
    [ TestFixture ]
    public class ImportModelConverterUnitTests
    {
        [ SetUp ]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }

        [ Test ]
        public void Convert__ProductImportModelIn_ProductOut__ReturnsProduct ()
        {

            // Arrange:
            var converter = new ImportModelConverter< ProductImportModel, Product >( GetMockedVisitor() );
            var productImportModel = new ProductImportModel();

            // Action:
            var product = converter.Convert( productImportModel );

            // Assert:
            Assert.That( product.GetType().IsAssignableFrom( typeof(Product) ) );
        }

        [Test]
        public void Convert__GatheringImportModelIn_GatheringActionOut__ReturnsGatheringAction ()
        {

            // Arrange:
            var converter = new ImportModelConverter< GatheringImportModel, GatheringAction >( GetMockedVisitor() );
            var gatheringImportModel = new GatheringImportModel();

            // Action:
            var gatheringAction = converter.Convert( gatheringImportModel );

            // Assert:
            Assert.That( gatheringAction.GetType().IsAssignableFrom( typeof( GatheringAction ) ) );
        }




        #region Factory

        private IImportModelVisitor GetMockedVisitor ()
        {
            var visitor = new Mock< IImportModelVisitor >();

            visitor.Setup( v => v.GetDbModel( It.IsAny< ProductImportModel >() ) ).Returns( new Product() );
            visitor.Setup( v => v.GetDbModel( It.IsAny< GatheringImportModel >() ) ).Returns( new GatheringAction() );

            return visitor.Object;
        }

        #endregion
    }
}
