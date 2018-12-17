using System.Globalization;
using NUnit.Framework;
using WorkSpeed.Data.Comparers;
using WorkSpeed.Data.Models;
using WorkSpeed.ProductivityIndicatorsModels;

// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable RedundantBoolCompare

namespace WorkSpeed.Tests.Comparers
{
    [TestFixture]
    public class EmployeeComparerUnitTests
    {
        [SetUp]
        public void SetupCulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo ("en-us");
        }

        [Test]
        public void Equals__EmployeeXIsNull_EmployeeYIsNull__ReturnsTrue()
        {
            var comparer = new EmployeeComparer();
            Employee employeeX = null;
            Employee employeeY = null;

            var res = comparer.Equals (employeeX, employeeY);

            Assert.That (true == res);
        }

        [Test]
        public void Equals__EmployeeXIsNotNull_EmployeeYIsNull__ReturnsFalse()
        {
            var comparer = new EmployeeComparer();
            Employee employeeX = new Employee();
            Employee employeeY = null;

            var res = comparer.Equals (employeeX, employeeY);

            Assert.That (false == res);
        }

        [Test]
        public void Equals__EmployeeXIsNull_EmployeeYIsNotNull__ReturnsFalse()
        {
            var p = new ProductivityTime();

            var comparer = new EmployeeComparer();
            Employee employeeX = null;
            Employee employeeY = new Employee();

            var res = comparer.Equals (employeeX, employeeY);

            Assert.That (false == res);
        }

        [Test]
        public void Equals__EmployeesHaveEqualId__ReturnsFalse()
        {
            var comparer = new EmployeeComparer();
            Employee employeeX = new Employee { Id = "AR12345", Rank = new Rank {Number = 3} };
            Employee employeeY = new Employee { Id = "AR12345", Rank = new Rank {Number = 2} };

            var res = comparer.Equals (employeeX, employeeY);

            Assert.That (true == res);
        }
    }
}
