using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Business.Constraints;
using WorkSpeed.Business.ProductivityIndicatorsModels;
using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.Data.Models.Enums;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public class Productivity2
    {
        #region Static

        private static readonly Dictionary<OperationGroups, string> _indicatorsNames;

        static Productivity2 ()
        {
            _indicatorsNames = new Dictionary< OperationGroups, string >
            {
                [ OperationGroups.Gathering ] = "Набор неклиентского товара",
                //[ OperationGroups.ClientGathering ] = "Набор клиентского товара",
                //[ OperationGroups.ShopperGathering ] = "Набор товара покупателей",

                //[ OperationGroups.Placing ] = "Расстановка",
                //[ OperationGroups.Defragmentation ] = "Подтоварка",
                //[ OperationGroups.Inventory ] = "Инвентаризация",

                //[ OperationGroups.Scanning ] = "Сканирование неклиентского товара",
                //[ OperationGroups.ClientScanning ] = "Сканирование клиентского товара",

                [ OperationGroups.Shipment ] = "Загружено/Погружено",
            };
        }

        #endregion


        #region Filds

        protected readonly TimeIndicators _times;
        protected readonly CompositeQuantityIndicators _gathered;
        protected readonly LineIndicators _placed;
        protected readonly LineIndicators _defragment;
        protected readonly LineIndicators _inventory;
        protected readonly CompositeQuantityIndicators _scanned;
        protected readonly CompositeQuantityIndicators _shipment;

        private readonly ITimeConstraints _timeConstraints;
        private readonly ICategoryConstraints _categoryConstraints;

        #endregion


        #region Constructor

        public Productivity2 ( Employee employee )
        {
            Employee = employee ?? throw new ArgumentNullException();

            _timeConstraints = new DefaultTimeConstraints();
            _categoryConstraints = new DefaultCategoryConstraints();

            _times = new TimeIndicators( "Рабочее время", _timeConstraints );

            _gathered = new CompositeQuantityIndicators( "Набор", _categoryConstraints );
            FillGetheringIndicators( _gathered );

            _placed = new LineIndicators( _indicatorsNames[ OperationGroups.Placing ], _categoryConstraints );
            _defragment = new LineIndicators( _indicatorsNames[ OperationGroups.Defragmentation ], _categoryConstraints );
            _inventory = new LineIndicators( _indicatorsNames[ OperationGroups.Inventory ], _categoryConstraints );

            _scanned = new CompositeQuantityIndicators( "Сканирование" );
            //FillScanningIndicators( _scanned );

            _shipment = new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Shipment ] );
            //FillShipmentIndicators( _shipment );
        }

        private void FillGetheringIndicators ( CompositeQuantityIndicators gathered )
        {
            // gathering
            gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Gathering ] ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            //    // client gathering
            //    gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientGathering ] ) );

            //    ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
            //        .AddIndicators( new LineIndicators( "Строчки" ) );

            //    ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
            //        .AddIndicators( new VolumeIndicators( "Объём" ) );


            //    // shopper gathering
            //    gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ShopperGathering ] ) );

            //    ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ShopperGathering ] ] )
            //        .AddIndicators( new LineIndicators( "Строчки" ) );

            //    ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ShopperGathering ] ] )
            //        .AddIndicators( new VolumeIndicators( "Объём" ) );
            //}

            //private void FillScanningIndicators ( CompositeQuantityIndicators scanned )
            //{
            //    scanned.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Scanning ] ) );

            //    ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.Scanning ] ] )
            //        .AddIndicators( new LineIndicators( "Строчки" ) );

            //    ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.Scanning ] ] )
            //        .AddIndicators( new VolumeIndicators( "Объём" ) );


            //    scanned.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientScanning ] ) );

            //    ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.ClientScanning ] ] )
            //        .AddIndicators( new LineIndicators( "Строчки" ) );

            //    ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.ClientScanning ] ] )
            //        .AddIndicators( new VolumeIndicators( "Объём" ) );
        }

        private void FillShipmentIndicators ( CompositeQuantityIndicators shipment )
        {
            shipment.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Shipment ] ) );

            ( ( CompositeQuantityIndicators )shipment[ _indicatorsNames[ OperationGroups.Shipment ] ] )
                .AddIndicators( new WeightIndicators( "Объём" ) );

            ( ( CompositeQuantityIndicators )shipment[ _indicatorsNames[ OperationGroups.Shipment ] ] )
                .AddIndicators( new CargoIndicators( "Места" ) );
        }

        #endregion


        #region Properties

        public Employee Employee { get; }

        public ProductivityIndicators Times => _times;
        public ProductivityIndicators Gathered => _gathered;
        public ProductivityIndicators Placed => _placed;
        public ProductivityIndicators Defragment => _defragment;
        public ProductivityIndicators Inventory => _inventory;
        public ProductivityIndicators Scanned => _scanned;
        public ProductivityIndicators Shipment => _shipment;

        #endregion


        #region Methods

        public void AddTime ( EmployeeActionBase employeeAction,  AddTimeOptions option = AddTimeOptions.Duration )
        {
            switch ( employeeAction.Operation.Group ) {

                case OperationGroups.Gathering :
                case OperationGroups.Packing :

                    (( TimeIndicators )Times).GatheringTime += 
                        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).GatheringTime );
                    break;

                //case OperationGroups.ClientGathering :
                //case OperationGroups.ClientPacking :

                //    (( TimeIndicators )Times).ClientGatheringTime += 
                //        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).ClientGatheringTime );
                //    break;

                //case OperationGroups.ShopperGathering:

                //    (( TimeIndicators )Times).ShopperGatheringTime += 
                //        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).ShopperGatheringTime );
                //    break;

                //case OperationGroups.Scanning :

                //    (( TimeIndicators )Times).ScanningTime += 
                //        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).ScanningTime );
                //    break;

                //case OperationGroups.ClientScanning :

                //    (( TimeIndicators )Times).ClientScanningTime += 
                //        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).ClientScanningTime );
                //    break;

                case OperationGroups.Defragmentation :

                    (( TimeIndicators )Times).DefragmentationTime += 
                        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).DefragmentationTime );
                    break;

                case OperationGroups.Placing :

                    (( TimeIndicators )Times).PlacingTime += 
                        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).PlacingTime );
                    break;

                case OperationGroups.Inventory :

                    (( TimeIndicators )Times).InventoryTime += 
                        GetProductivityTimer( employeeAction, option, (( TimeIndicators )Times).InventoryTime );
                    break;

                case OperationGroups.Shipment :

                    (( TimeIndicators )Times).ShipmentTime += 
                        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).ShipmentTime );
                    break;

                default :

                    (( TimeIndicators )Times).NonProductivTime += 
                        GetProductivityTimer( employeeAction,  option,  (( TimeIndicators )Times).NonProductivTime );
                    break;
            }
        }

        private ProductivityTime GetProductivityTimer ( EmployeeActionBase employeeAction,  AddTimeOptions option,  ProductivityTime timer )
        {
            switch ( option ) {

                case AddTimeOptions.Continuous :

                    var endTime = employeeAction.StartTime.Add( employeeAction.Duration );
                    timer.Duration = endTime - timer.EndTime;
                    timer.EndTime = endTime;

                    break;

                case AddTimeOptions.Duration :

                    timer.Duration += employeeAction.Duration;
                    timer.EndTime = employeeAction.StartTime.Add(employeeAction.Duration);

                    break;
            }

            return timer;
        }


        #endregion
    }
}
