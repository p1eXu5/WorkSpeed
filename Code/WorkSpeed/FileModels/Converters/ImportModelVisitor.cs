using System;
using WorkSpeed.Data.Models;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;

namespace WorkSpeed.FileModels.Converters
{
    public class ImportModelVisitor : IImportModelVisitor
    {
        public object GetDbModel ( ImportModel importModel )
        {

            return new object();
        }


        public Product GetDbModel( ProductImportModel productImportModel )
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

        public Employee GetDbModel ( EmployeeImportModel employeeImportModel )
        {
            return new Employee {

                Id = employeeImportModel.EmployeeId,
                Name = employeeImportModel.EmployeeName,
            };
        }

        public Employee GetDbModel ( EmployeeFullImportModel employeeImportModel )
        {
            return new Employee {

                Id = employeeImportModel.EmployeeId,
                Name = employeeImportModel.EmployeeName,
            };
        }

        public GatheringAction GetDbModel ( GatheringImportModel gatheringImportModel )
        {
            GatheringAction gatheringAction = GetWithProductAction< GatheringAction >( gatheringImportModel );

            gatheringAction.SenderCellAdress = GetAddress( gatheringImportModel.AddressSender );
            gatheringAction.ReceiverCellAdress = GetAddress( gatheringImportModel.AddressReceiver );

            return gatheringAction;
        }

        public ReceptionAction GetDbModel ( ReceptionImportModel receptionImportModel )
        {
            ReceptionAction reseptionAction = GetWithProductAction< ReceptionAction >( receptionImportModel );

            reseptionAction.ScanQuantity = receptionImportModel.ScanQuantity;

            reseptionAction.ReceiverCellAddress = GetAddress( receptionImportModel.Address );
            reseptionAction.IsClientScanning = receptionImportModel.IsClientScanning;

            return reseptionAction;
        }

        public InventoryAction GetDbModel ( InventoryImportModel inventoryImportModel )
        {
            InventoryAction inventoryAction = GetWithProductAction< InventoryAction >( inventoryImportModel );

            inventoryAction.AccountingQuantity = inventoryImportModel.AccountingQuantity;
            inventoryAction.InventoryCellAddress = GetAddress( inventoryImportModel.Address );

            return inventoryAction;
        }

        public ShipmentAction GetDbModel ( ShipmentImportModel shipmentImportModel )
        {
            ShipmentAction shipmentAction = GetEmployeeAction< ShipmentAction >( shipmentImportModel );

            shipmentAction.Weight = ( float )shipmentImportModel.WeightPerEmployee;
            shipmentAction.ClientCargoQuantity = ( float )shipmentImportModel.ClientCargoQuantity;
            shipmentAction.CommonCargoQuantity = ( float )shipmentAction.CommonCargoQuantity;

            return shipmentAction;
        }

        public EmployeeAction GetDbModel ( ProductivityImportModel productivityImportModel )
        {
            var receiverAddress = productivityImportModel.ReceiverAddress;
            var senderAddress = productivityImportModel.SenderAddress;

            if ( String.IsNullOrWhiteSpace( receiverAddress ) && String.IsNullOrWhiteSpace( senderAddress ) ) {

                ShipmentImportModel shipmentImportModel = GetImportModel< ShipmentImportModel >( productivityImportModel );
                shipmentImportModel.WeightPerEmployee = productivityImportModel.WeightPerEmployee;
                shipmentImportModel.ClientCargoQuantity = productivityImportModel.CommonCargoQuantity;
                shipmentImportModel.CommonCargoQuantity = productivityImportModel.CommonCargoQuantity;

                return GetDbModel( shipmentImportModel );
            }
            else if ( String.IsNullOrWhiteSpace( receiverAddress ) ) {

                InventoryImportModel inventoryImportModel = GetImportModel< InventoryImportModel >( productivityImportModel );
                inventoryImportModel.Address = productivityImportModel.SenderAddress;
                inventoryImportModel.AccountingQuantity = productivityImportModel.AccountingQuantity;

                return GetDbModel( inventoryImportModel );
            }
            else if ( String.IsNullOrWhiteSpace( senderAddress ) ) {

                ReceptionImportModel receptionImportModel = GetImportModel< ReceptionImportModel >( productivityImportModel );
                receptionImportModel.Address = productivityImportModel.ReceiverAddress;
                receptionImportModel.IsClientScanning = productivityImportModel.IsClientScanning;
                receptionImportModel.ScanQuantity = productivityImportModel.ScanQuantity;

                return GetDbModel( receptionImportModel );
            }
            else {

                GatheringImportModel gatheringImportModel = GetImportModel< GatheringImportModel >( productivityImportModel );
                gatheringImportModel.AddressSender = productivityImportModel.SenderAddress;
                gatheringImportModel.AddressReceiver = productivityImportModel.ReceiverAddress;

                return GetDbModel( gatheringImportModel );
            }
        }


        private TImportModel GetWithProductAction< TImportModel >( WithProductActionImportModel withProductModel )
            where TImportModel : WithProductAction
        {
            var withProductAction = GetEmployeeAction< WithProductAction >( withProductModel );
            var importModel = ( TImportModel )Activator.CreateInstance( typeof( TImportModel ) );

            importModel.Product = new Product {

                Id = withProductModel.ProductId,
                Name = withProductModel.Product,

                Parent = new Product {

                    Id = withProductModel.ImmadiateProductId,
                    Name = withProductModel.ImmadiateProduct,

                    Parent = new Product {

                        Id = withProductModel.SecondProductId,
                        Name = withProductModel.SecondProduct
                    }
                }
            };

            withProductAction.ProductQuantity = withProductModel.ProductQuantity;

            return importModel;
        }

        private TImportModel GetEmployeeAction< TImportModel >( ActionImportModel actionImportModel )
            where TImportModel : EmployeeAction
        {
            var importModel = ( TImportModel )Activator.CreateInstance( typeof( TImportModel ) );

            importModel.StartTime = actionImportModel.StartTime;

            importModel.Employee = new Employee {

                Id = actionImportModel.EmployeeId,
                Name = actionImportModel.EmployeeName,
            };

            importModel.Duration = TimeSpan.FromSeconds( actionImportModel.OperationDuration );

            importModel.Document = new Document1C {

                Id = actionImportModel.DocumentNumber,
                Name = actionImportModel.DocumentName,
                Date = actionImportModel.StartTime
            };

            importModel.Operation = new Operation {
                Name = actionImportModel.Operation
            };

            return importModel;
        }

        private TImportType GetImportModel< TImportType > ( ActionImportModel actionImportModel )
            where TImportType : ActionImportModel
        {
            var importModel = ( TImportType )Activator.CreateInstance( typeof( TImportType ) );

            importModel.StartTime = actionImportModel.StartTime;
            importModel.DocumentNumber = actionImportModel.DocumentNumber;
            importModel.DocumentName = actionImportModel.DocumentName;
            importModel.EmployeeId = actionImportModel.EmployeeId;
            importModel.EmployeeName = actionImportModel.EmployeeName;
            importModel.Operation = actionImportModel.Operation;
            importModel.OperationDuration = actionImportModel.OperationDuration;

            return importModel;
        }


        private Address GetAddress ( string address )
        {
            try {
                return new Address {

                    Letter = address[ 0 ],
                    Row = Byte.Parse( address.Substring( 1, 2 ) ),
                    Section = Byte.Parse( address.Substring( 4, 2 ) ),
                    Shelf = Byte.Parse( address.Substring( 7, 2 ) ),
                    CellNum = Byte.Parse( address.Substring( 10, 2 ) ),
                };
            }
            catch {
                return new Address();
            }
        }
    }
}
