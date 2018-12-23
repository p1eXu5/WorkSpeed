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

            _canMoveNext = true;
        }

        public override int StageNum { get; } = 3;
        public override string Header { get; } = "Проверка сотрудников.";

        public override string Message { get; protected set; }

        public ReadOnlyObservableCollection< EmployeeViewModel > Employees { get; }

        public IEnumerable< Appointment > GetAppointments => Warehouse.GetAppointments();
        public IEnumerable< Position > GetPositions => Warehouse.GetPositions();
        public IEnumerable< Rank > GetRanks => Warehouse.GetRanks();

    }
}
