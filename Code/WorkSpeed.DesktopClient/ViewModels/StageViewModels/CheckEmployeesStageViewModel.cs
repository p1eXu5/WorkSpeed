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

        public CheckEmployeesStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum )
        {
            _employees = new ObservableCollection< EmployeeViewModel >( Warehouse.GetEmployees().Select( e => new EmployeeViewModel( e ) ) );
            Employees = new ReadOnlyObservableCollection< EmployeeViewModel >( _employees );

            Positions = new List< Position >( Warehouse.GetPositions() );
            Ranks = new List< Rank >( Warehouse.GetRanks() );

            _canMoveNext = true;
        }

        public override string Header { get; } = "Проверка сотрудников.";

        public ReadOnlyObservableCollection< EmployeeViewModel > Employees { get; }

        public IEnumerable< Appointment > Appointments => Warehouse.GetAppointments();
        public List< Position > Positions { get; set; }
        public List< Rank > Ranks { get; set; }

    }
}
