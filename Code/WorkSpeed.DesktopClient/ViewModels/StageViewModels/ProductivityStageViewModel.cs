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
        public ProductivityStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base( fastProductivityViewModel ) { }
        public override string Header { get; } = "Выработка.";
        public override int StageNum { get; } = 4;
    }
}
