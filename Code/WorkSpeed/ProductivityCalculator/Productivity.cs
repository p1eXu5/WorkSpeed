using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Attributes;
using WorkSpeed.Constraints;
using WorkSpeed.Data.Models;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed.ProductivityCalculator
{
    public class Productivity
    {
        #region Static

        private static readonly Dictionary<OperationGroups, string> _indicatorsNames;

        static Productivity ()
        {
            _indicatorsNames = new Dictionary< OperationGroups, string >
            {
                [ OperationGroups.Gathering ] = "Набор неклиентского товара",
                [ OperationGroups.ClientGathering ] = "Набор клиентского товара",
                [ OperationGroups.ShopperGathering ] = "Набор товара покупателей",

                [ OperationGroups.Placing ] = "Расстановка",
                [ OperationGroups.Defragmentation ] = "Подтоварка",
                [ OperationGroups.Inventory ] = "Инвентаризация",

                [ OperationGroups.Scanning ] = "Сканирование неклиентского товара",
                [ OperationGroups.ClientScanning ] = "Сканирование клиентского товара",

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

        #endregion


        #region Constructor

        public Productivity ( EmployeeAction employeeAction,  ITimeConstraintFactory timeConstraintFactory,  ICategoryConstraints categoryConstraints )
        {
            Employee = employeeAction.Employee ?? throw new ArgumentNullException();

            _times = new TimeIndicators( "Рабочее время", timeConstraintFactory.GeTimeConstraint( Employee ) );

            _gathered = new CompositeQuantityIndicators( "Набор", categoryConstraints );
            FillGetheringIndicators( _gathered );

            _placed = new LineIndicators( _indicatorsNames[ OperationGroups.Placing ], categoryConstraints );
            _defragment = new LineIndicators( _indicatorsNames[ OperationGroups.Defragmentation ], categoryConstraints );
            _inventory = new LineIndicators( _indicatorsNames[ OperationGroups.Inventory ], categoryConstraints );

            _scanned = new CompositeQuantityIndicators( "Сканирование" );
            FillScanningIndicators( _scanned );

            _shipment = new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Shipment ] );
            FillShipmentIndicators( _shipment );

            AddTime( employeeAction );
            AddActionDetails( employeeAction );
        }

        private void FillGetheringIndicators ( CompositeQuantityIndicators gathered )
        {
            // gathering
            gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Gathering ] ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            // client gathering
            gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientGathering ] ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            // shopper gathering
            gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ShopperGathering ] ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ShopperGathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )gathered[ _indicatorsNames[ OperationGroups.ShopperGathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );
        }

        private void FillScanningIndicators ( CompositeQuantityIndicators scanned )
        {
            scanned.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Scanning ] ) );

            ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.Scanning ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.Scanning ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            scanned.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientScanning ] ) );

            ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.ClientScanning ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )scanned[ _indicatorsNames[ OperationGroups.ClientScanning ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );
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

        public void AddTime ( EmployeeAction employeeAction,  AddTimeOptions option = AddTimeOptions.Duration )
        {
            switch ( employeeAction.Operation.OperationGroup ) {

                case OperationGroups.Gathering :
                case OperationGroups.Packing :

                    Times.GatheringTime += GetProductivityTimer( employeeAction, option, Times.GatheringTime );
                    break;

                case OperationGroups.ClientGathering :
                case OperationGroups.ClientPacking :

                    Times.ClientGatheringTime += GetProductivityTimer( employeeAction, option, Times.ClientGatheringTime );
                    break;

                case OperationGroups.ShopperGathering:

                    Times.ShopperGatheringTime += GetProductivityTimer( employeeAction, option, Times.ShopperGatheringTime );
                    break;

                case OperationGroups.Scanning :

                    Times.ScanningTime += GetProductivityTimer( employeeAction, option, Times.ScanningTime );
                    break;

                case OperationGroups.ClientScanning :

                    Times.ClientScanningTime += GetProductivityTimer( employeeAction, option, Times.ClientScanningTime );
                    break;

                case OperationGroups.Defragmentation :

                    Times.DefragmentationTime += GetProductivityTimer( employeeAction, option, Times.DefragmentationTime );
                    break;

                case OperationGroups.Placing :

                    Times.PlacingTime += GetProductivityTimer( employeeAction, option, Times.PlacingTime );
                    break;

                case OperationGroups.Inventory :

                    Times.InventoryTime += GetProductivityTimer( employeeAction, option, Times.InventoryTime );
                    break;

                case OperationGroups.Shipment :

                    Times.ShipmentTime += GetProductivityTimer( employeeAction, option, Times.ShipmentTime );
                    break;

                default :

                    Times.NonProductivTime += GetProductivityTimer( employeeAction, option, Times.NonProductivTime );
                    break;
            }
        }

        public void AddActionDetails ( EmployeeAction employeeAction )
        {

            switch ( employeeAction ) {

                case GatheringAction gatheringAction :

                    (()Gathered).AddQuantity( gatheringAction );
                    break;
            }
        }

        private ProductivityTime GetProductivityTimer ( EmployeeAction employeeAction,  AddTimeOptions option,  ProductivityTime timer )
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

        private void AddGatheringDetails ( GatheringAction gatheringAction )
        {
            
        }

        #endregion
    }
}
