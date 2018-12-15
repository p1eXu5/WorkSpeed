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
        public object GetDbModel ( ImportModel importModel )
        {
            //  *********************************
            //  To fill:
            //          - Product data
            //          - OperationGroup
            //          - Action
            //
            //  *********************************

            //switch ( withProductModel ) {
                    
            //    case ProductImportModel productImportModel :
            //        return GetProduct( productImportModel );

            //    case EmployeeImportModel employeeImportModel :
            //        return GetEmployee( employeeImportModel );

            //    case GatheringImportModel gatheringImportModel :
            //        return GetGatheringAction( gatheringImportModel );

            //    case ReceptionImportModel receptionImportModel:
            //        return GetReception
            //}

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

        public GatheringAction GetDbModel ( GatheringImportModel gatheringImportModel )
        {
            GatheringAction gatheringAction = ( GatheringAction )GetWithProductAction( gatheringImportModel );

            gatheringAction.SenderCellAdress = GetAddress( gatheringImportModel.AddressSender );
            gatheringAction.ReceiverCellAdress = GetAddress( gatheringImportModel.AddressReceiver );

            return gatheringAction;
        }

        public ReceptionAction GetDbModel ( ReceptionImportModel receptionImportModel )
        {
            ReceptionAction reseptionAction = ( ReceptionAction )GetWithProductAction( receptionImportModel );

            reseptionAction.ScanQuantity = receptionImportModel.ScanQuantity;

            reseptionAction.ReceiverCellAddress = GetAddress( receptionImportModel.Address );
            reseptionAction.IsClientScanning = receptionImportModel.IsClientScanning;

            return reseptionAction;
        }

        public InventoryAction GetDbModel ( InventoryImportModel inventoryImportModel )
        {
            InventoryAction inventoryAction = ( InventoryAction )GetWithProductAction( inventoryImportModel );

            inventoryAction.AccountingQuantity = inventoryImportModel.AccountingQuantity;
            inventoryAction.InventoryCellAddress = GetAddress( inventoryImportModel.Address );

            return inventoryAction;
        }

        public ShipmentAction GetDbModel ( ShipmentImportModel shipmentImportModel )
        {
            ShipmentAction shipmentAction = ( ShipmentAction )GetEmployeeAction( shipmentImportModel );

        }

        public EmployeeAction GetDbModel ( ProductivityImportModel productivityImportModel )
        {
            var receiverAddress = productivityImportModel.AddressReceiver;
            var senderAddress = productivityImportModel.AddressSender;

            if ( String.IsNullOrWhiteSpace( receiverAddress ) && String.IsNullOrWhiteSpace( senderAddress ) ) {

                ShipmentImportModel shipmentImportModel = GetImportModel< ShipmentImportModel >( productivityImportModel );
                shipmentImportModel.WeightPerEmployee = productivityImportModel.WeightPerEmployee;
                shipmentImportModel.ClientCargoQuantity = productivityImportModel.CommonCargoQuantity;
                shipmentImportModel.CommonCargoQuantity = productivityImportModel.CommonCargoQuantity;

                return GetDbModel( shipmentImportModel );
            }

            return (EmployeeAction)new object();
        }

        private EmployeeAction GetEmployeeAction ( ActionImportModel actionImportModel )
        {
            return new WithProductAction {

                StartTime = actionImportModel.StartTime,

                Employee = new Employee {

                    Id = actionImportModel.EmployeeId,
                    Name = actionImportModel.EmployeeName,
                },

                Duration = TimeSpan.FromSeconds( actionImportModel.OperationDuration ),

                Document = new Document1C {

                    Id = actionImportModel.DocumentNumber,
                    Name = actionImportModel.DocumentName,
                    Date = actionImportModel.StartTime
                },

                Operation = new Operation {
                    Name = actionImportModel.Operation
                },
            };
        }

        private WithProductAction GetWithProductAction ( WithProductActionImportModel withProductModel )
        {
            WithProductAction withProductAction = ( WithProductAction )GetEmployeeAction( withProductModel );

            withProductAction.Product = new Product {

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

            return withProductAction;
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
