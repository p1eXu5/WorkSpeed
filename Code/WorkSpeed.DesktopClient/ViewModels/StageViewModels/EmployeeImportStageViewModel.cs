using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    class EmployeeImportStageViewModel : ImportStageViewModel
    {
        public EmployeeImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) 
            : base( fastProductivityViewModel )
        { }

        public override int StageNum { get; } = 1;
        public override string Header { get; } = "Сотрудники. Импорт.";

        protected override async void Open ( object obj )
        {
            var fileName = base.OpenExcelFile();

            if ( String.IsNullOrWhiteSpace( fileName ) ) return;

            FileName = fileName;

            IsInProgress = true;
            ProgressCounter = 0;
            Message = "Загрузка...";

            var productsLastCount = Warehouse.GetEmployees().Count();

            var cancellationToken = GetCancellationToken();
            var progress = new Progress<double>( ( d ) => ProgressCounter = d );

            bool areEmployeesAdded = await Warehouse.ImportAsync< EmployeeFullImportModel >( fileName, cancellationToken, progress );

            if ( !areEmployeesAdded ) {
                areEmployeesAdded = await Warehouse.ImportAsync<EmployeeImportModel>( fileName, cancellationToken, progress );
            }

            if ( areEmployeesAdded ) {

                Message = $"Добавлено { Warehouse.GetEmployees().Count() - productsLastCount } сотрудников";
                UpdateCanForward();
            }
            else {

                IsInProgress = false;
                Message = "Файл не содержит позиций номенклатуры.";
            }
        }

        protected override bool CanForward ( object obj )
        {
            return Warehouse.GetEmployees().Any();
        }
    }
}
