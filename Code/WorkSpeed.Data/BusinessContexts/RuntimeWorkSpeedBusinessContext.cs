﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly List< Appointment > _appointments = new List<Appointment>();
        private readonly List< Position > _positions = new List<Position>();
        private readonly List< GatheringAction > _gatheringAction;

        public RuntimeWorkSpeedBusinessContext ()
        {
            OnModelCreating();
        }

        private void OnModelCreating ()
        {
            SeedOperationGroups();
            SeedOperations();
            SeedAppointments();
            SeedPositions();
        }

        private void SeedOperations ()
        {
            _operations.AddRange( new[] {
                new Operation {
                    Id = 1,
                    Name = "Выгрузка машины",
                    OperationGroupId = 1,
                    Group = _operationGroups.First( o => o.Id == 1 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 2,
                    Name = "Погрузка машины",
                    OperationGroupId = 1,
                    Group = _operationGroups.First( o => o.Id == 1 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 3,
                    Name = "Подбор товара",
                    OperationGroupId = 2,
                    Group = _operationGroups.First( o => o.Id == 2 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 4,
                    Name = "Подбор клиентского товара",
                    OperationGroupId = 4,
                    Group = _operationGroups.First( o => o.Id == 4 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 5,
                    Name = "Подбор товара покупателей",
                    OperationGroupId = 4,
                    Group = _operationGroups.First( o => o.Id == 4 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 6,
                    Name = "Упаковка товара в места",
                    OperationGroupId = 3,
                    Group = _operationGroups.First( o => o.Id == 3 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 7,
                    Name = "Приём товара",
                    OperationGroupId = 6,
                    Group = _operationGroups.First( o => o.Id == 6 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 8,
                    Name = "Размещение товара",
                    OperationGroupId = 9,
                    Group = _operationGroups.First( o => o.Id == 9 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 9,
                    Name = "Перемещение товара",
                    OperationGroupId = 10,
                    Group = _operationGroups.First( o => o.Id == 10 ),
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 10,
                    Name = "Инвентаризация",
                    OperationGroupId = 11,
                    Group = _operationGroups.First( o => o.Id == 11 ),
                    Complexity = (float) 1.0
                },
            } );
        }

        private void SeedOperationGroups ()
        {
            _operationGroups.AddRange( new[] {
                new OperationGroup { Id = 1, Name = OperationGroups.Shipment, Complexity = (float) 1.0 },
                new OperationGroup { Id = 2, Name = OperationGroups.Gathering, Complexity = (float) 1.0 },
                new OperationGroup { Id = 3, Name = OperationGroups.Packing, Complexity = (float) 1.0 },
                new OperationGroup { Id = 4, Name = OperationGroups.ClientGathering, Complexity = (float) 1.0 },
                new OperationGroup { Id = 5, Name = OperationGroups.ClientPacking, Complexity = (float) 1.0 },
                new OperationGroup { Id = 6, Name = OperationGroups.Scanning, Complexity = (float) 1.0 },
                new OperationGroup { Id = 7, Name = OperationGroups.ClientScanning, Complexity = (float) 1.0 },
                new OperationGroup { Id = 8, Name = OperationGroups.Defragmentation, Complexity = (float) 1.0 },
                new OperationGroup { Id = 9, Name = OperationGroups.Placing, Complexity = (float) 1.0 },
                new OperationGroup { Id = 10, Name = OperationGroups.Replacing, Complexity = (float) 1.0 },
                new OperationGroup { Id = 11, Name = OperationGroups.Inventory, Complexity = (float) 1.0 },
            } );
        }

        private void SeedAppointments ()
        {
            _appointments.AddRange( new [] {

                new Appointment {

                    Id = 1,
                    InnerName = "Грузчик",
                    OfficialName = "Грузчик",
                    SalaryPerOneHour = 47.85m,
                    Abbreviations = "гр;гр.;груз;грузч.;"
                },

                new Appointment {

                    Id = 2,
                    InnerName = "Кладовщик на РРЦ",
                    OfficialName = "Кладовщик склада",
                    SalaryPerOneHour = 52.64m,
                    Abbreviations = "кладовщик;кл;кл.;клад;клад.;"
                },

                new Appointment {

                    Id = 3,
                    InnerName = "Кладовщик приемщик",
                    OfficialName = "Кладовщик-приемщик",
                    SalaryPerOneHour = 57.42m,
                    Abbreviations = "приёмщик;приемщик;пр;пр.;"
                },

                new Appointment {

                    Id = 4,
                    InnerName = "Водитель погрузчика",
                    OfficialName = "Водитель погрузчика",
                    SalaryPerOneHour = 52.64m,
                    Abbreviations = "водитель;вод;вод.;карщик;"
                },

                new Appointment {

                    Id = 5,
                    InnerName = "Старший кладовщик на РРЦ",
                    OfficialName = "Старший кладовщик склада",
                    SalaryPerOneHour = 62.21m,
                    Abbreviations = "старший;ст;ст.;старшийкладовщик;ст.клад."
                },

                new Appointment {

                    Id = 6,
                    InnerName = "Заместитель управляющего склада по отгрузке",
                    OfficialName = "Менеджер по отправке груза",
                    SalaryPerOneHour = 95.70m,
                    Abbreviations = "зампоприёмке;зам.пр."
                },

                new Appointment {

                    Id = 7,
                    InnerName = "Заместитель управляющего склада по приемке",
                    OfficialName = "Менеджер по приему груза",
                    SalaryPerOneHour = 92.22m,
                    Abbreviations = "зампоотгрузке;зам.отгр.;"
                },

                new Appointment {

                    Id = 8,
                    InnerName = "Управляющий РРЦ",
                    OfficialName = "Управляющий складом",
                    SalaryPerOneHour = 119.63m,
                    Abbreviations = "управляющий;упр.;упр;упр.складом"
                },

            } );
        }

        private void SeedPositions ()
        {
            _positions.AddRange( new [] {

                new Position {

                    Id = 1,
                    Name = "Крупняк",
                    Abbreviations = "кр.;кр;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 2,
                    Name = "Приёмка",
                    Abbreviations = "приемка;пр.;пр;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 3,
                    Name = "Отгрузка",
                    Abbreviations = "отгр.;отгр;от.;от;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 4,
                    Name = "Дорогая",
                    Abbreviations = "дор.;дор;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 5,
                    Name = "Мезонин, 1-й этаж",
                    Abbreviations = "мезонин1;мез1;мез.1;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 6,
                    Name = "Мезонин, 2-й этаж",
                    Abbreviations = "мезонин2;мез2;мез.2;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 7,
                    Name = "Мезонин, 3-й этаж",
                    Abbreviations = "мезонин3;мез3;мез.3;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 8,
                    Name = "Мезонин, 4-й этаж",
                    Abbreviations = "мезонин4;мез4;мез.4;",
                    Complexity = 1.0f,
                },

            } );
        }

        public bool HasProducts () => _products.Any();

        public Task< bool > HasProductsAsync ()
        {
            return Task.Factory.StartNew( () => _products.Any() );
        }

        public IEnumerable< Product > GetProducts () => _products;

        public void AddProduct ( Product product )
        {
            if ( _products.All( p => p.Id != product.Id ) ) {
                _products.Add( product );
            }
        }

        public ReadOnlyObservableCollection< GatheringAction > GatheringActions { get; }

        public void AddGatheringAction ( GatheringAction gatheringAction )
        {
            _gatheringAction.Add( gatheringAction );
        }
    }
}
