using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class CheckEmployeesStageViewModel : StageViewModel
    {
        private readonly ObservableCollection< EmployeeViewModel > _employees;

        public CheckEmployeesStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) 
            : base( fastProductivityViewModel )
        {
            _employees = new ObservableCollection< EmployeeViewModel >( Warehouse.GetEmployees().Select( e => new EmployeeViewModel( e ) ) );
            Employees = new ReadOnlyObservableCollection< EmployeeViewModel >( _employees );

            Appointments = new List< Appointment >( Warehouse.GetAppointments() );
            Positions = new List< Position >( Warehouse.GetPositions() );
            Ranks = new List< Rank >( Warehouse.GetRanks() );

            _canMoveNext = true;
        }

        public override int StageNum { get; } = 3;
        public override string Header { get; } = "Проверка сотрудников.";

        public override string Message { get; protected set; }

        public ReadOnlyObservableCollection< EmployeeViewModel > Employees { get; }

        public List< Appointment > Appointments { get; set; }
        public List< Position > Positions { get; set; }
        public List< Rank > Ranks { get; set; }

    }
}
