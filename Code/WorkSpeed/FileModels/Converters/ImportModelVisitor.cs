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
                Rank = new Rank
                {
                    Number = employeeImportModel.Rank ?? 3
                },

                Position = new Position
                {
                    Abbreviation = employeeImportModel.Position
                },
            };
        }

        public AllActions GetDbModel ( ProductivityImportModel productivityImportModel )
        {
            var actions = new AllActions();

            if ( productivityImportModel.ProductId == null ) {

                actions.ShipmentActions = GetShipmentAction( productivityImportModel );
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

        private ShipmentAction GetShipmentAction ( ProductivityImportModel shipmentImportModel )
        {
            var shipment = new ShipmentAction {

                Id = shipmentImportModel.DocumentNumber,
                DocumentName = shipmentImportModel.DocumentName,

                StartTime = shipmentImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( shipmentImportModel.OperationDuration ),
                Operation = new Operation { Name = shipmentImportModel.Operation },
                Employee = new Employee { Name = shipmentImportModel.EmployeeName },

                ShipmentActionDetail = new ShipmentActionDetail {
                    Weight = ( float? )shipmentImportModel.WeightPerEmployee,
                    Volume = ( float? )shipmentImportModel.VolumePerEmployee,
                    ClientCargoQuantity = ( float? )shipmentImportModel.ClientCargoQuantityt,
                    CommonCargoQuantity = ( float? )shipmentImportModel.CommonCargoQuantity
                }
            };

            return shipment;
        }

        private InventoryAction GetInventoryAction ( ProductivityImportModel inventoryImportModel )
        {
            var inventory = new InventoryAction {

                Id = inventoryImportModel.DocumentNumber,
                DocumentName = inventoryImportModel.DocumentName,

                StartTime = inventoryImportModel.StartTime,
                Duration = TimeSpan.FromSeconds( inventoryImportModel.OperationDuration ),
                Operation = new Operation { Name = inventoryImportModel.Operation },
                Employee = new Employee { Name = inventoryImportModel.EmployeeName },

                InventoryActionDetails = new List< InventoryActionDetail > {
                    new InventoryActionDetail {
                        AccountingQuantity = inventoryImportModel.AccountingQuantity ?? 0,
                        
                    }
                }
            };

            return inventory;
        }

        private ReceptionAction GetReceptionAction ( ProductivityImportModel productivityImportModel )
        {
            var reception = new ReceptionAction { };

            return reception;
        }

        private DoubleAddressAction GetDoubleAddressAction ( ProductivityImportModel productivityImportModel )
        {
            var doubleAction = new DoubleAddressAction { };

            return doubleAction;
        }
    }
}
