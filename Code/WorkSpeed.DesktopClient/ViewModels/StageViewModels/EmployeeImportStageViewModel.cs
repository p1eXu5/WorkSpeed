using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    class EmployeeImportStageViewModel : ImportStageViewModel
    {
        public EmployeeImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base( fastProductivityViewModel ) { }
        public override string Header { get; } = "Сотрудники. Импорт.";
        public override string Message { get; protected set; }
        public override int StageNum { get; } = 1;

        protected override void Open ( object obj )
        {
            throw new NotImplementedException();
        }
    }
}
