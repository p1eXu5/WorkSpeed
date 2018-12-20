using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class ProductivityStageViewModel : StageViewModel
    {
        public ProductivityStageViewModel ( IWarehouse warehouse ) : base( warehouse ) { }
        public override string Header { get; } = "Выработка.";
        public override int StageNum { get; } = 4;
    }
}
