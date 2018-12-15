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

            switch ( importModel ) {
                    
                case ProductImportModel productImportModel:
                    return GetProduct( productImportModel );
            }

            return new object();
        }

        public Product GetProduct( ProductImportModel productImportModel )
        {
            return new Product{

                Id = productImportModel.Id,
                Name = productImportModel.Name,

                GatheringComplexity = (float)1.0,
                InventoryComplexity = (float)1.0,
                PackagingComplexity = (float)1.0,
                PlacingComplexity = (float)1.0,
                ScanningComplexity = (float)1.0,

                CartonLength = ( float )productImportModel.CartonLength,
                CartonWidth = ( float )productImportModel.CartonWidth,
                CartonHeight = ( float )productImportModel.CartonHeight,
                CartonQuantity = productImportModel.CartonQuantity,
                ItemLength = ( float )productImportModel.ItemLength,
                ItemWidth = ( float )productImportModel.CartonWidth,
                ItemHeight = ( float )productImportModel.ItemHeight,
                Weight = ( float )productImportModel.Weight
            };
        }
    }
}
