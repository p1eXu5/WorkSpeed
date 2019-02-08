using System.Collections.Generic;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Comparers
{
    public static class ComparerFactory
    {
        private static IEqualityComparer< Employee > _employeeComparer;
        private static IEqualityComparer< Product > _productComparer;

        public static IEqualityComparer< Employee > EmployeeComparer 
            => _employeeComparer ?? (_employeeComparer = GetEmployeeComparer());

        public static IEqualityComparer< Product > ProductComparer 
            => _productComparer ?? (_productComparer = GetProductComparer());

        private static IEqualityComparer< Employee > GetEmployeeComparer()
            => new EntityEqualityComparer< Employee, string >();

        private static IEqualityComparer< Product > GetProductComparer()
            => new EntityEqualityComparer< Product, int >();
    }
}
