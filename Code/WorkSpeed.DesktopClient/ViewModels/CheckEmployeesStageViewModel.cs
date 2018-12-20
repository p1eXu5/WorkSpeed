using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class CheckEmployeesStageViewModel : StageViewModel
    {
        public CheckEmployeesStageViewModel ( IWarehouse warehouse ) : base( warehouse ) { }
        public override string Header { get; } = "Проверка сотрудников.";
        public override int StageNum { get; } = 3;
    }
}
