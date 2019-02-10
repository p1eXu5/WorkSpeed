using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Data.BusinessContexts
{
    public class RuntimeWorkSpeedBusinessContext : IWorkSpeedBusinessContext
    {
        #region Fields

        private readonly List< Product > _products = new List< Product >( 40_000 );
        private readonly List< Employee > _employees = new List< Employee >();
        private readonly List< DoubleAddressAction > _gatheringActions = new List< DoubleAddressAction >();
        private readonly List< Operation > _operations = new List< Operation >();
        private readonly List< Appointment > _appointments = new List< Appointment >();
        private readonly List< Position > _positions = new List< Position >();
        private readonly List< Rank > _ranks = new List< Rank >();
        private readonly List< Category > _categories = new List< Category >();
        private readonly List< Shift > _shifts = new List< Shift >();
        private readonly List< ShortBreakSchedule > _breaks = new List< ShortBreakSchedule >();

        #endregion


        #region Constructor

        public RuntimeWorkSpeedBusinessContext ()
        {
            OnModelCreating();
        }

        #endregion


        #region Seeding

        private void OnModelCreating ()
        {
            SeedOperations();
            SeedAppointments();
            SeedPositions();
            SeedRanks();
            SeedCategories();
            SeedShiftsAndShortBreakList();
        }


        private void SeedOperations ()
        {
            _operations.AddRange( new[] {
                new Operation {
                    Id = 1,
                    Name = "Выгрузка машины",
                    OperationGroup = OperationGroups.Shipment,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 2,
                    Name = "Погрузка машины",
                    OperationGroup = OperationGroups.Shipment,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 3,
                    Name = "Подбор товара",
                    OperationGroup = OperationGroups.Gathering,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 4,
                    Name = "Подбор клиентского товара",
                    OperationGroup = OperationGroups.Gathering,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 5,
                    Name = "Подбор товаров покупателей",
                    OperationGroup = OperationGroups.Gathering,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 6,
                    Name = "Упаковка товара в места",
                    OperationGroup = OperationGroups.Packing,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 7,
                    Name = "Приём товара",
                    OperationGroup = OperationGroups.Reception,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 8,
                    Name = "Размещение товара",
                    OperationGroup = OperationGroups.Placing,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 9,
                    Name = "Перемещение товара",
                    OperationGroup = OperationGroups.Placing,
                    Complexity = (float) 1.0
                },

                new Operation {
                    Id = 10,
                    Name = "Инвентаризация",
                    OperationGroup = OperationGroups.Inventory,
                    Complexity = (float) 1.0
                },
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
                    Abbreviation = "кр.;кр;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 2,
                    Name = "Приёмка",
                    Abbreviation = "приемка;пр.;пр;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 3,
                    Name = "Отгрузка",
                    Abbreviation = "отгр.;отгр;от.;от;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 4,
                    Name = "Дорогая",
                    Abbreviation = "дор.;дор;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 5,
                    Name = "Мезонин, 1-й этаж",
                    Abbreviation = "мезонин1;мез1;мез.1;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 6,
                    Name = "Мезонин, 2-й этаж",
                    Abbreviation = "мезонин2;мез2;мез.2;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 7,
                    Name = "Мезонин, 3-й этаж",
                    Abbreviation = "мезонин3;мез3;мез.3;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 8,
                    Name = "Мезонин, 4-й этаж",
                    Abbreviation = "мезонин4;мез4;мез.4;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 9,
                    Name = "Расстановка",
                    Abbreviation = "расстановка;рас;рас.;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 10,
                    Name = "Старший смены, мезонин",
                    Abbreviation = "ссммез;",
                    Complexity = 1.0f,
                },

                new Position {

                    Id = 11,
                    Name = "Старший смены, крупняк",
                    Abbreviation = "ссмкр;",
                    Complexity = 1.0f,
                },
            } );
        }

        private void SeedRanks ()
        {
            _ranks.AddRange( new[] {

                new Rank {

                    Number = 2,
                    OneHourCost = 163m
                },

                new Rank {

                    Number = 3,
                    OneHourCost = 180m
                },

                new Rank {

                    Number = 4,
                    OneHourCost = 200m
                },

                new Rank {

                    Number = 5,
                    OneHourCost = 220m
                },

                new Rank {

                    Number = 6,
                    OneHourCost = 242.42m
                },

                new Rank {

                    Number = 7,
                    OneHourCost = 266.67m
                },

                new Rank {

                    Number = 8,
                    OneHourCost = 300m
                },

                new Rank {

                    Number = 9,
                    OneHourCost = 342.42m
                },

                new Rank {

                    Number = 10,
                    OneHourCost = 366.66m
                },

                new Rank {

                    Number = 11,
                    OneHourCost = 400m
                },

                new Rank {

                    Number = 12,
                    OneHourCost = 442.42m
                },

                new Rank {

                    Number = 13,
                    OneHourCost = 466.66m
                },

                new Rank {

                    Number = 14,
                    OneHourCost = 533.33m
                },
            } );
        }

        private void SeedCategories ()
        {
            _categories.AddRange( new [] {
                new Category {
                    Id = 1,
                    Name = "Товары до 2,5 литров",
                    MinVolume = 0.0,
                    MaxVolume = 2.5
                },
                new Category {
                    Id = 2,
                    Name = "Товары до 5 литров",
                    MinVolume = 2.5,
                    MaxVolume = 5.0,
                },
                new Category {
                    Id = 3,
                    Name = "Товары до 25 литров",
                    MinVolume = 5.0,
                    MaxVolume = 25.0,
                },
                new Category {
                    Id = 4,
                    Name = "Товары до 100 литров",
                    MinVolume = 25.0,
                    MaxVolume = 100.0,
                },
                new Category {
                    Id = 5,
                    Name = "Товары до 250 литров",
                    MinVolume = 100.0,
                    MaxVolume = 250.0,
                },
                new Category {
                    Id = 6,
                    Name = "Товары от 250 литров",
                    MinVolume = 250.0,
                    MaxVolume = double.PositiveInfinity,
                },
            } );
        }

        private void SeedShiftsAndShortBreakList ()
        {
            _shifts.AddRange( new [] {
                new Shift {
                    Id = 1,
                    Name = "Дневная смена",
                    StartTime = new TimeSpan( 8, 0, 0),
                    Duration = TimeSpan.FromHours( 12 ),
                    Lunch = TimeSpan.FromMinutes( 30 ),
                    RestTime = TimeSpan.FromMinutes( 60 )
                },
                new Shift {
                    Id = 2,
                    Name = "Ночная смена",
                    StartTime = new TimeSpan( 20, 0, 0),
                    Duration = TimeSpan.FromHours( 12 ),
                    Lunch = TimeSpan.FromMinutes( 30 ),
                    RestTime = TimeSpan.FromMinutes( 60 )
                },
            } );

            _breaks.AddRange( new [] {
                new ShortBreakSchedule {
                    Id = 1,
                    Name = "Перекуры для некурящих",
                    Duration = TimeSpan.FromMinutes( 10 ),
                    Periodicity = TimeSpan.FromHours( 2 ),
                    FirstBreakTime = new TimeSpan( 9, 55, 0 ),
                },
                new ShortBreakSchedule {
                    Id = 2,
                    Name = "Перекуры для курящих",
                    Duration = TimeSpan.FromMinutes( 5 ),
                    Periodicity = TimeSpan.FromHours( 1 ),
                    FirstBreakTime = new TimeSpan( 8, 55, 0),
                },
            } );
        }

        #endregion


        #region Methods

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

        public IEnumerable<DoubleAddressAction> GetGatheringActions () => _gatheringActions.OrderBy( a => a.StartTime );

        public IEnumerable<DoubleAddressAction> GetGatheringActions ( Employee employee )
        {
            if ( employee == null ) throw new ArgumentNullException();

            return _gatheringActions.Where( a => a.Employee.Id == employee.Id ).OrderBy( a => a.StartTime );
        }

        public void AddGatheringAction (DoubleAddressAction gatheringAction )
        {
            if ( gatheringAction == null
                 || gatheringAction.Operation == null
                 || String.IsNullOrEmpty( gatheringAction.Operation.Name )
                 || _gatheringActions.Contains( gatheringAction )
                 || gatheringAction.Employee == null) {

                return;
            }

            var employee = _employees.FirstOrDefault( e => e.Id.Equals( gatheringAction.Employee.Id ) );
            if ( employee == null ) return;
            gatheringAction.Employee = employee;

            var operation = _operations.FirstOrDefault( o => o.Name.Equals( gatheringAction.Operation.Name ) );
            if (operation == null) return;
            gatheringAction.Operation = operation;;

            _gatheringActions.Add( gatheringAction );
        }

        public IEnumerable< Employee > GetEmployees () => _employees;

        public void AddEmployee ( Employee employee )
        {
            if ( employee == null || employee.Id.Length != 7 || _employees.Contains( employee ) ) return;

            if ( employee.Appointment == null || String.IsNullOrWhiteSpace( employee.Appointment.Abbreviations ) ) return;
            var appointment = _appointments.FirstOrDefault( a => a.Abbreviations.Contains( employee.Appointment.Abbreviations ) );
            if (appointment == null) return;
            employee.Appointment = appointment;

            if ( employee.Position == null || String.IsNullOrWhiteSpace(employee.Position.Abbreviation) ) return;
            var position = _positions.FirstOrDefault( p => p.Abbreviation.Contains( employee.Position.Abbreviation ) );
            if ( position == null ) return;
            employee.Position = position;

            if ( employee.Rank == null || employee.Rank.Number <= 0 || employee.Rank.Number > _ranks.Max( r => r.Number ) ) return;
            var rank = _ranks.FirstOrDefault( r => r.Number == employee.Rank.Number );
            if ( rank == null ) return;
            employee.Rank = rank;

            _employees.Add( employee );
        }

        public IEnumerable< Appointment > GetAppointments ()
        {
            return _appointments;
        }

        public IEnumerable< Position > GetPositions ()
        {
            return _positions;
        }

        public IEnumerable< Rank > GetRanks ()
        {
            return _ranks;
        }

        public IEnumerable< Category > GetCategories ()
        {
            return _categories;
        }

        public IEnumerable< Shift > GetShifts ()
        {
            return _shifts;
        }

        public IEnumerable< ShortBreakSchedule > GetBreakList ()
        {
            return _breaks;
        }

        #endregion
    }
}
