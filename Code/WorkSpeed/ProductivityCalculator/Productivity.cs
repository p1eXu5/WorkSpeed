using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Attributes;
using WorkSpeed.Data.Models;
using WorkSpeed.ProductivityIndicatorsModels;

namespace WorkSpeed.ProductivityCalculator
{
    public class Productivity
    {
        private static readonly Dictionary<
            OperationGroups,
            Action <
                Productivity,
                EmployeeAction,
                AddTimeOptions
            >
        > _addTimeMap;

        static Productivity ()
        {
            _addTimeMap = new Dictionary<OperationGroups, Action<Productivity, EmployeeAction, AddTimeOptions>>()
            {
                [ OperationGroups.Gathering ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.GatheringTime ),
                [ OperationGroups.Packing ] = _addTimeMap[ OperationGroups.Gathering ],

                [ OperationGroups.ClientGathering ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.ClientGatheringTime ),
                [ OperationGroups.ClientPacking ] = _addTimeMap[ OperationGroups.ClientGathering ],

                [ OperationGroups.Scanning ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.ScanningTime ),
                [ OperationGroups.ClientScanning ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.ClientScanningTime ),

                [ OperationGroups.Placing ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.PlacingTime ),
                [ OperationGroups.Replacing ] = _addTimeMap[ OperationGroups.Placing ],

                [ OperationGroups.Defragmentation ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.DefragmentationTime ),
                [ OperationGroups.Inventory ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.InventorizationTime ),
                [ OperationGroups.Shipment ] = ( productivity, action, timer ) => productivity.AddTime( action, timer, ref productivity.Times.ShipmentTime ),
            };
        }

        public Productivity ( Employee employee, ICategoryConstraints categoryConstraints )
        {
            Employee = employee ?? throw new ArgumentNullException();

            Gathered = new CompositeQuantityIndicators();
            Gathered.AddIndicators(  );
        }

        public Productivity ( EmployeeAction employeeAction )
        {
            Employee = employeeAction.Employee;
            AddTime( employeeAction );
            AddActionDetails( employeeAction );
        }

        public Employee Employee { get; }

        [Header("Время работы")]
        public TimeIndicators Times { get; set; } = new TimeIndicators();

        [ Header( "Собрано" ) ]
        public CompositeQuantityIndicators Gathered { get; set; }

        [Header("Расставлено")]
        public ICollection<ProductivityIndicators> Placed { get; set; } = new List<ProductivityIndicators>();
        [Header("Подтоварено")]
        public ICollection<ProductivityIndicators> Defragmented { get; set; } = new List<ProductivityIndicators>();
        [Header("Проинвентарено")]
        public ICollection<ProductivityIndicators> Inventory { get; set; } = new List<ProductivityIndicators>();
        [Header("Просканировано")]
        public ICollection<ProductivityIndicators> Scanned { get; set; } = new List<ProductivityIndicators>();
        [Header("Загружено/Погружено")]
        public ICollection<ProductivityIndicators> LoadedUnloaded { get; set; } = new List<ProductivityIndicators>();

        public void AddTime ( EmployeeAction employeeAction,  AddTimeOptions option = AddTimeOptions.Duration )
        {
            _addTimeMap[employeeAction.Operation.OperationGroup].Invoke( this, employeeAction, option );
        }

        public void AddActionDetails ( EmployeeAction employeeAction )
        {

            switch ( employeeAction ) {

                case GatheringAction gatheringAction :

                    AddGatheringDetails( gatheringAction );
                    break;
            }
        }

        private void AddTime ( EmployeeAction employeeAction,  AddTimeOptions option,  ref ProductivityTimer timer )
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

                case AddTimeOptions.NonProductiveTime :

                    timer.Duration += employeeAction.Duration;
                    timer.EndTime = employeeAction.StartTime.Add(employeeAction.Duration);
                    Times.NonProductiveTime.Duration += employeeAction.StartTime - timer.EndTime;

                    break;
            }
        }

        private void AddGatheringDetails ( GatheringAction gatheringAction )
        {
            _gatheringDetailsMap[ gatheringAction.Operation.OperationGroup ] += gatheringAction;
        }
    }
}
