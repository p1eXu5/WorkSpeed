using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class CategoriesThresholdStageViewModel : StageViewModel
    {
        public CategoriesThresholdStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum )
        { }

        public override string Header { get; } = "Настройка категорий и порогов";
    }
}
