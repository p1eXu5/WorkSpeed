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
        private static readonly Dictionary<OperationGroups, string> _indicatorsNames;

        static Productivity ()
        {
            _indicatorsNames = new Dictionary< OperationGroups, string >
            {
                [ OperationGroups.ClientGathering ] = "Набор клиентского товара",
                [ OperationGroups.Gathering ] = "Набор неклиентского товара",
                [ OperationGroups.Packing ] = "",
            };
        }

        public Productivity ( Employee employee,  IBreakConstraints breakConstraints,  ICategoryConstraints categoryConstraints )
        {
            Employee = employee ?? throw new ArgumentNullException();

            FillGetheringIndicators( categoryConstraints );
        }

        private void FillGetheringIndicators ( ICategoryConstraints categoryConstraints )
        {
            Gathered = new CompositeQuantityIndicators( "Набор", categoryConstraints );


            Gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.Gathering ] ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.Gathering ] ] )
                .AddIndicators( new QuantityIndicators( "Объём" ) );


            Gathered.AddIndicators( new CompositeQuantityIndicators( _indicatorsNames[ OperationGroups.ClientGathering ] ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
                .AddIndicators( new LineIndicators( "Строчки" ) );

            ( ( CompositeQuantityIndicators ) Gathered[ _indicatorsNames[ OperationGroups.ClientGathering ] ] )
                .AddIndicators( new QuantityIndicators( "Объём" ) );
        }

        public Productivity ( EmployeeAction employeeAction )
        {
            Employee = employeeAction.Employee;
            AddTime( employeeAction );
            AddActionDetails( employeeAction );
        }

        public Employee Employee { get; }

        [Header("Время работы")]
        public TimeIndicators Times { get; set; } = new TimeIndicators( "Рабочее время" );

        [ Header( "Собрано" ) ]
        public CompositeQuantityIndicators Gathered { get; set; }

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

        private ProductivityTimer GetProductivityTimer ( EmployeeAction employeeAction,  AddTimeOptions option,  ProductivityTimer timer )
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
    }
}
