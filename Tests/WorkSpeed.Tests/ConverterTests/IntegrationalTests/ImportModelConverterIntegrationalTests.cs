using System.Globalization;
using NUnit.Framework;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;
using WorkSpeed.FileModels.Converters;

namespace WorkSpeed.Tests.ConverterTests.IntegrationalTests
{
    [ TestFixture ]
    public class ImportModelConverterIntegrationalTests
    {
        [SetUp]
        public void SetupCulture ()
        {
            CultureInfo.CurrentUICulture = new CultureInfo( "en-us" );
        }

        [ Test ]
        public void Convert__ProductImportModelIn_ProductOut__ReturnsProduct ()
        {
            // Arrange:
            var converter = new ImportModelConverter< ProductImportModel, Product >( new ImportModelVisitor() );

            var productImportModel = new ProductImportModel {

                Id = 7123,
                Name = "Product A",
                Weight = 1.123
            };

            // Action:
            var product = converter.Convert( productImportModel );

            // Assert:
            Assert.That( product.GetType().IsAssignableFrom( typeof( Product ) ) );
            Assert.That( 7123 == product.Id );
            Assert.That( product.Name.Equals( "Product A") );
            Assert.That( product.Weight.Equals( ( float )1.123 ) );
        }
    }
}
