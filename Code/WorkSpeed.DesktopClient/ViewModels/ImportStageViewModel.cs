using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Interfaces;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public abstract class ImportStageViewModel : StageViewModel
    {
        protected ImportStageViewModel ( IWarehouse warehouse ) : base( warehouse ) { }
    }
}
