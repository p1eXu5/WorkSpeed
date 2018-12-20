using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    class ProductImportStageViewModel : ImportStageViewModel, IStageViewModel
    {
        public ProductImportStageViewModel ( IWarehouse warehouse ) : base( warehouse ) { }
        public override string Header { get; } = "Номенклатура. Импорт.";
        public override int StageNum { get; } = 0;
    }
}
