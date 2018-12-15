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
        private readonly IWorkSpeedData _productsAndOperations;

        public ImportModelVisitor( IWorkSpeedData productsAndOperations )
        {
            _productsAndOperations = productsAndOperations ?? throw new ArgumentNullException( nameof(productsAndOperations) );
        }

        public object GetDbModel ( ImportModel importModel )
        {
            //  *********************************
            //  To fill:
            //          - Product data
            //          - OperationGroup
            //          - Action
            //
            //  *********************************

            EmployeeAction employeeAction = null;

            switch ( importModel ) {
                    
                case ProductImportModel productImportModel:
                    return GetProduct( productImportModel );
            }

            return new object();
        }

        public Product GetProduct( ProductImportModel productImportModel )
        {
            var newProduct = new Product();

            // TODO:

            return newProduct;
        }
    }
}
