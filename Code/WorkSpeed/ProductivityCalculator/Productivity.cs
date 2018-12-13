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


        #region Constructor

        public Productivity ( EmployeeAction employeeAction,  ITimeConstraintFactory timeConstraintFactory,  ICategoryConstraints categoryConstraints )
        {
            Employee = employeeAction.Employee ?? throw new ArgumentNullException();

            Times = new TimeIndicators( "Рабочее время", timeConstraintFactory.GeTimeConstraint( Employee ) );

            FillGetheringIndicators( categoryConstraints );

            Placed = new LineIndicators( _indicatorsNames[ OperationGroups.Placing ], categoryConstraints );
            Defragment = new LineIndicators( _indicatorsNames[ OperationGroups.Defragmentation ], categoryConstraints );
            Inventory = new LineIndicators( _indicatorsNames[ OperationGroups.Inventory ], categoryConstraints );

            FillScanningIndicators( categoryConstraints );

            FillShipmentIndicators( categoryConstraints );

            AddTime( employeeAction );
            AddActionDetails( employeeAction );
        }

        private void FillGetheringIndicators ( ICategoryConstraints categoryConstraints )
        {
            // root
            Gathered = new CompositeQuantityIndicators( "Набор", categoryConstraints );


            // gathering
            Gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Gathering ] ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            // client gathering
            Gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientGathering ] ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            // shopper gathering
            Gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ShopperGathering ] ) );

            ( ( CompositeQuantityIndicators )Gathered[ _indicatorsNames[ OperationGroups.ShopperGathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )Gathered[ _indicatorsNames[ OperationGroups.ShopperGathering ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );
        }

        private void FillScanningIndicators ( ICategoryConstraints categoryConstraints )
        {
            Scanned = new CompositeQuantityIndicators( "Сканирование" );


            Scanned.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Scanning ] ) );

            ( ( CompositeQuantityIndicators )Scanned[ _indicatorsNames[ OperationGroups.Scanning ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )Scanned[ _indicatorsNames[ OperationGroups.Scanning ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );


            Scanned.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientScanning ] ) );

            ( ( CompositeQuantityIndicators )Scanned[ _indicatorsNames[ OperationGroups.ClientScanning ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators )Scanned[ _indicatorsNames[ OperationGroups.ClientScanning ] ] )
                .AddIndicators( new VolumeIndicators( "Объём" ) );
        }

        private void FillShipmentIndicators ( ICategoryConstraints categoryConstraints )
        {
            Shipment = new CompositeQuantityIndicators( "Сканирование" );


            Shipment.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Scanning ] ) );

            ( ( CompositeQuantityIndicators )Shipment[ _indicatorsNames[ OperationGroups.Shipment ] ] )
                .AddIndicators( new WeightIndicators( "Объём" ) );

            ( ( CompositeQuantityIndicators )Shipment[ _indicatorsNames[ OperationGroups.Shipment ] ] )
                .AddIndicators( new CargoIndicators( "Места" ) );
        }

        #endregion


        #region Properties

        public Employee Employee { get; }

        [Header("Время работы")]
        public TimeIndicators Times { get; }

        [ Header( "Собрано" ) ]
        public CompositeQuantityIndicators Gathered { get; private set; }

        [Header( "Расставлено" )]
        public LineIndicators Placed { get; private set; }

        [Header( "Подтоварено" )]
        public LineIndicators Defragment { get; private set; }

        [Header( "Проинвентарено" )]
        public LineIndicators Inventory { get; private set; }

        [Header( "Просканировано" )]
        public CompositeQuantityIndicators Scanned { get; private set; }

        [Header( "Загружено/Погружено" )]
        public CompositeQuantityIndicators Shipment { get; private set; }

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

                    AddGatheringDetails( gatheringAction );
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
