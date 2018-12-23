using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class ProductivityStageViewModel : StageViewModel
    {
        public ProductivityStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum )
        { }

        public override string Header { get; } = "Выработка.";
        public override string Message { get; protected set; }
    }
}
