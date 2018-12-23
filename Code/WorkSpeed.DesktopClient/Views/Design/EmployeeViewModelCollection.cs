using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using WorkSpeed.DesktopClient.ViewModels;
using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;

namespace WorkSpeed.DesktopClient.Views.Design
{
    public class EmployeeViewModelCollection : ObservableCollection< EmployeeViewModel >
    {
        public EmployeeViewModelCollection ()
        {

            Add( new EmployeeViewModel( new Employee {
                Id = "AR12345",
                Name = "Ландин Михаил Вячеславович",
                IsActive = true,
            } ) );

            Add( new EmployeeViewModel( new Employee
            {
                Id = "AR65420",
                Name = "Наконечный Александр Владимирович",
                IsActive = true,
            } ) );

            Add( new EmployeeViewModel( new Employee
            {
                Id = "BY12013",
                Name = "Шведов Владимир Викторович",
                IsActive = true,
            } ) );

            Add( new EmployeeViewModel( new Employee
            {
                Id = "AR02983",
                Name = "Реуцкий Евгений Васильевич",
                IsActive = true,
            } ) );
        }


    }
}
