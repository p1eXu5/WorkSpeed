using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class CheckEmployeesStageViewModel : StageViewModel
    {
        public CheckEmployeesStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base( fastProductivityViewModel ) { }
        public override string Header { get; } = "Проверка сотрудников.";
        public override string Message { get; protected set; }
        public override int StageNum { get; } = 3;
    }
}
