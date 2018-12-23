using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class ActionsImportStageViewModel : ImportStageViewModel
    {
        public ActionsImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum ) { }

        public override string Header { get; }

        protected override void Open ( object obj )
        {
            throw new NotImplementedException();
        }
    }
}
