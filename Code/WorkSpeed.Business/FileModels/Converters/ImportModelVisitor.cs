using System;
using System.Collections.Generic;
using WorkSpeed.Business.Models;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.ActionDetails;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.FileModels.Converters
{
    public class ImportModelVisitor : IImportModelVisitor
    {
        public IEntity GetDbModel( ImportModel importModel )
        {
            return GetDbModel( (dynamic) importModel );
        }

        /// <summary>
        /// Creates Product from ProductImportModel
        /// </summary>
        /// <param name="productImportModel"><see cref="ProductImportModel"/></param>
        /// <returns><see cref="Product"/></returns>
        public Product GetDbModel ( ProductImportModel productImportModel )
        {
            return new Product
            {
                Id = productImportModel.Id,
                Name = productImportModel.Name,
                IsGroup = false,

                ItemLength = (float?)productImportModel.ItemLength,
                ItemWidth = (float?)productImportModel.ItemWidth,
                ItemHeight = (float?)productImportModel.ItemHeight,
                ItemWeight = (float?)productImportModel.ItemWeight,

                CartonLength = (float?)productImportModel.CartonLength,
                CartonWidth = (float?)productImportModel.CartonWidth,
                CartonHeight = (float?)productImportModel.CartonHeight,
                CartonQuantity = productImportModel.CartonQuantity,
                
                GatheringComplexity = (float)1.0,
                InventoryComplexity = (float)1.0,
                PackagingComplexity = (float)1.0,
                PlacingComplexity = (float)1.0,
                ScanningComplexity = (float)1.0,
            };
        }

        public Employee GetDbModel( EmployeeImportModel employeeImportModel )
        {
            return new Employee
            {
                Id = employeeImportModel.EmployeeId,
                Name = employeeImportModel.EmployeeName,
                IsActive = employeeImportModel.IsActive,

                Appointment = new Appointment { Abbreviations = employeeImportModel.Appointment },
                Rank = new Rank { Number = employeeImportModel.Rank ?? 3 },
                Position = new Position { Abbreviations = employeeImportModel.Position },
            };
        }

        public AllActions GetDbModel ( ProductivityImportModel productivityImportModel )
        {
            var actions = new AllActions();

            if ( productivityImportModel.ProductId == null ) {

                if ( productivityImportModel.ClientCargoQuantityt == null
                     && productivityImportModel.CommonCargoQuantity == null
                     && productivityImportModel.WeightPerEmployee == null
                     && productivityImportModel.VolumePerEmployee == null ) {

                    actions.OtherAction = GetOtherAction( productivityImportModel );
                }
                else {
                    actions.ShipmentAction = GetShipmentAction( productivityImportModel );
                }
            }
            else if ( !string.IsNullOrWhiteSpace( productivityImportModel.SenderAddress )
                      && string.IsNullOrWhiteSpace( productivityImportModel.ReceiverAddress ) ) {

                actions.InventoryAction = GetInventoryAction( productivityImportModel );
            }
            else if ( string.IsNullOrWhiteSpace( productivityImportModel.SenderAddress )
                      && !string.IsNullOrWhiteSpace( productivityImportModel.ReceiverAddress ) ) {

                actions.ReceptionAction = GetReceptionAction( productivityImportModel );
            }
            else {
                actions.DoubleAddressAction = GetDoubleAddressAction( productivityImportModel );
            }

            return actions;
        }



        private static OtherAction GetOtherAction ( ProductivityImportModel productivityImportModel )
        {
            var other = new OtherAction {

                Id = productivityImportModel.DocumentNumber.Trim(),
                DocumentName = productivityImportModel.DocumentName,

                StartTime = productivityImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( productivityImportModel.OperationDuration ),
                Operation = new Operation { Name = productivityImportModel.Operation.Trim() },
                Employee = GetEmployee( productivityImportModel ),
            };

            return other;
        }

        private static ShipmentAction GetShipmentAction ( ProductivityImportModel productivityImportModel )
        {
            var id = productivityImportModel.DocumentNumber.Trim();
            var shipment = new ShipmentAction {

                Id = id,
                DocumentName = productivityImportModel.DocumentName,

                StartTime = productivityImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( productivityImportModel.OperationDuration ),
                Operation = new Operation { Name = productivityImportModel.Operation.Trim() },
                EmployeeId = productivityImportModel.EmployeeId,
                Employee = GetEmployee( productivityImportModel ),

                Weight = ( float? )productivityImportModel.WeightPerEmployee,
                Volume = ( float? )productivityImportModel.VolumePerEmployee,
                ClientCargoQuantity = ( float? )productivityImportModel.ClientCargoQuantityt,
                CommonCargoQuantity = ( float? )productivityImportModel.CommonCargoQuantity
            };

            return shipment;
        }

        private static InventoryAction GetInventoryAction ( ProductivityImportModel productivityImportModel )
        {
            var inventory = new InventoryAction {

                Id = productivityImportModel.DocumentNumber.Trim(),
                DocumentName = productivityImportModel.DocumentName,

                StartTime = productivityImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( productivityImportModel.OperationDuration ),
                Operation = new Operation { Name = productivityImportModel.Operation.Trim() },
                Employee = GetEmployee( productivityImportModel ),

                InventoryActionDetails = new List< InventoryActionDetail > {
                    new InventoryActionDetail {
                        ProductId = productivityImportModel.ProductId ?? 0,
                        Product = GetProduct( productivityImportModel ),
                        ProductQuantity = productivityImportModel.ProductQuantity ?? 0,
                        Address = GetAddress( productivityImportModel.SenderAddress ),
                        AccountingQuantity = productivityImportModel.AccountingQuantity ?? 0,
                    }
                }
            };

            return inventory;
        }

        private static ReceptionAction GetReceptionAction ( ProductivityImportModel productivityImportModel )
        {
            var reception = new ReceptionAction {

                Id = productivityImportModel.DocumentNumber.Trim(),
                DocumentName = productivityImportModel.DocumentName,

                StartTime = productivityImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( productivityImportModel.OperationDuration ),
                Operation = new Operation { Name = productivityImportModel.Operation.Trim() },
                Employee = GetEmployee( productivityImportModel ),

                ReceptionActionDetails = new List< ReceptionActionDetail > {
                    new ReceptionActionDetail {
                        ProductId = productivityImportModel.ProductId ?? 0,
                        Product = GetProduct( productivityImportModel ),
                        Address = GetAddress( productivityImportModel.ReceiverAddress ),
                        ScanQuantity = (short)(productivityImportModel.ScanQuantity ?? 0),
                        IsClientScanning = productivityImportModel.IsClientScanning ?? false
                    }
                }
            };

            return reception;
        }

        private static DoubleAddressAction GetDoubleAddressAction ( ProductivityImportModel productivityImportModel )
        {
            var doubleAction = new DoubleAddressAction {

                Id = productivityImportModel.DocumentNumber.Trim(),
                DocumentName = productivityImportModel.DocumentName,

                StartTime = productivityImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( productivityImportModel.OperationDuration ),
                Operation = new Operation { Name = productivityImportModel.Operation.Trim() },
                Employee = GetEmployee( productivityImportModel ),

                DoubleAddressDetails = new List< DoubleAddressActionDetail > {
                    new DoubleAddressActionDetail {
                        ProductId = productivityImportModel.ProductId ?? 0,
                        Product = GetProduct( productivityImportModel ),
                        ProductQuantity = productivityImportModel.ProductQuantity ?? 0,
                        SenderAddress = GetAddress( productivityImportModel.SenderAddress ),
                        ReceiverAddress = GetAddress( productivityImportModel.ReceiverAddress )
                    }
                }
            };

            return doubleAction;
        }



        private static Product GetProduct ( ProductivityImportModel productivityImportModel )
        {
            var product = new Product {

                Id = productivityImportModel.ProductId ?? 0,
                Name = productivityImportModel.Product.Trim(),
            };

            return product;
        }

        private static Employee GetEmployee ( ProductivityImportModel productivityImportModel )
        {
            return new Employee {
                Id = productivityImportModel.EmployeeId.Trim(),
                Name = productivityImportModel.EmployeeName
            };
        }

        private static Address GetAddress ( string addressName )
        {
            var address = new Address();

            if ( string.IsNullOrWhiteSpace( addressName ) || addressName.Length != 12 ) {
                return address;
            }

            try {
                address.Letter = addressName.Substring( 0, 1 );
                address.Row = Convert.ToByte( addressName.Substring( 1, 2 ) );
                address.Section = Convert.ToByte( addressName.Substring( 4, 2 ) );
                address.Shelf = Convert.ToByte( addressName.Substring( 7, 2 ) );
                address.Box = Convert.ToByte( addressName.Substring( 10, 2 ) );
            }
            catch ( Exception ) {
                return new Address();
            }

            return address;
        }
    }
}
