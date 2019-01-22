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
            Assert.That( product.ItemWeight.Equals( ( float )1.123 ) );
        }

        [Test]
        public void Convert__EmployeeFullImportModelIn_EmployeeOut__ReturnsEmployeeWithExpectedValues ()
        {
            // Arrange:
            var converter = new ImportModelConverter< EmployeeFullImportModel, Employee >( new ImportModelVisitor() );

            var employeeFullImportModel = new EmployeeFullImportModel()
            {
                EmployeeId = "AR12345",
                EmployeeName = "Вася Пупкин",
                IsActive = true,
                Position = "пр",
                Appointment = "кл",
                Rank = 5,
            };

            // Action:
            var employee = converter.Convert( employeeFullImportModel );

            // Assert:
            Assert.That( employee.GetType().IsAssignableFrom( typeof( Employee ) ) );
            Assert.That( "AR12345".Equals( employee.Id ) );
            Assert.That( "Вася Пупкин".Equals( employee.Name ) );
            Assert.That( true == employee.IsActive );
            Assert.That( "пр".Equals( employee.Position.Abbreviation ) );
            Assert.That( "кл".Equals( employee.Appointment.Abbreviations ) );
            Assert.That( 5 == employee.Rank.Number );
           
        }
    }
}
