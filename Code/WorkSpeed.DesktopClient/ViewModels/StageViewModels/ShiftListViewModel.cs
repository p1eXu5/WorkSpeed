using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class ShiftListViewModel : StageViewModel
    {
        public ShiftListViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum )
        { }

        public override string Header { get; } = "Настройка смен";
    }
}
