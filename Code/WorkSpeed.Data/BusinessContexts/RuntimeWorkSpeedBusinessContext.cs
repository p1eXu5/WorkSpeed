using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.BusinessContexts
{
    public class RuntimeWorkSpeedBusinessContext : IWorkSpeedBusinessContext
    {
        private readonly List< Product > _products = new List< Product >( 40_000 );
        private readonly List< OperationGroup > _operationGroups = new List< OperationGroup >();
        private readonly List< Operation > _operations = new List< Operation >();

        public RuntimeWorkSpeedBusinessContext ()
        {
            OnModelCreating();
        }

        private void OnModelCreating ()
        {
            _operationGroups.AddRange( new [] {

                new OperationGroup { Id = 1,    Name = OperationGroups.Shipment,           Complexity = ( float )1.0 },
                new OperationGroup { Id = 2,    Name = OperationGroups.Gathering,          Complexity = ( float )1.0 },
                new OperationGroup { Id = 3,    Name = OperationGroups.Packing,            Complexity = ( float )1.0 },
                new OperationGroup { Id = 4,    Name = OperationGroups.ClientGathering,    Complexity = ( float )1.0 },
                new OperationGroup { Id = 5,    Name = OperationGroups.ClientPacking,      Complexity = ( float )1.0 },
                new OperationGroup { Id = 6,    Name = OperationGroups.Scanning,           Complexity = ( float )1.0 },
                new OperationGroup { Id = 7,    Name = OperationGroups.ClientScanning,     Complexity = ( float )1.0 },
                new OperationGroup { Id = 8,    Name = OperationGroups.Defragmentation,    Complexity = ( float )1.0 },
                new OperationGroup { Id = 9,    Name = OperationGroups.Placing,            Complexity = ( float )1.0 },
                new OperationGroup { Id = 10,   Name = OperationGroups.Replacing,          Complexity = ( float )1.0 },
                new OperationGroup { Id = 11,   Name = OperationGroups.Inventory,          Complexity = ( float )1.0 },
            } );

            _operations.AddRange( new[] {
                
                new Operation {
                    Id = 1, 
                    Name = "Выгрузка машины", 
                    OperationGroupId = 1, 
                    Group = _operationGroups.First( o => o.Id == 1 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 2, 
                    Name = "Погрузка машины", 
                    OperationGroupId = 1, 
                    Group = _operationGroups.First( o => o.Id == 1 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 3, 
                    Name = "Подбор товара", 
                    OperationGroupId = 2, 
                    Group = _operationGroups.First( o => o.Id == 2 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 4, 
                    Name = "Подбор клиентского товара", 
                    OperationGroupId = 4, 
                    Group = _operationGroups.First( o => o.Id == 4 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 5, 
                    Name = "Подбор товара покупателей", 
                    OperationGroupId = 4, 
                    Group = _operationGroups.First( o => o.Id == 4 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 6, 
                    Name = "Упаковка товара в места", 
                    OperationGroupId = 3, 
                    Group = _operationGroups.First( o => o.Id == 3 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 7, 
                    Name = "Приём товара", 
                    OperationGroupId = 6, 
                    Group = _operationGroups.First( o => o.Id == 6 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 8, 
                    Name = "Размещение товара", 
                    OperationGroupId = 9, 
                    Group = _operationGroups.First( o => o.Id == 9 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 9, 
                    Name = "Перемещение товара", 
                    OperationGroupId = 10, 
                    Group = _operationGroups.First( o => o.Id == 10 ),
                    Complexity = ( float )1.0
                },

                new Operation {
                    Id = 10, 
                    Name = "Инвентаризация", 
                    OperationGroupId = 11, 
                    Group = _operationGroups.First( o => o.Id == 11 ),
                    Complexity = ( float )1.0
                },
            } );
        }

        public Task< bool > HasProductsAsync ()
        {
            return Task.Factory.StartNew( () => _products.Any() );
        }
    }
}
