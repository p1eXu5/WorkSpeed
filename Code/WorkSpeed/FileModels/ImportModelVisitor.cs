using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels
{
    public class ImportModelVisitor : IImportModelVisitor
    {
        private readonly IProductsAndOperations _productsAndOperations;

        public ImportModelVisitor( IProductsAndOperations productsAndOperations )
        {
            _productsAndOperations = productsAndOperations ?? throw new ArgumentNullException( nameof(productsAndOperations) );
        }

        public EmployeeAction ToEmployeeAction ( ImportModel importModel )
        {
            //  *********************************
            //  To fill:
            //          - Product data
            //          - OperationGroup
            //
            //  *********************************

            EmployeeAction employeeAction = null;

            switch ( importModel ) {
                    
                case ActionProductImportModel withProductImportModel:
                    return GetEmployeeAction( withProductImportModel );

                case GatheringImportModel gatheringImportModel:
                    return GetEmployeeAction( gatheringImportModel );
            }
        }

        public Product ToProduct( ProductImportModel importModel )
        {
            throw new NotImplementedException();
        }
    }
}
