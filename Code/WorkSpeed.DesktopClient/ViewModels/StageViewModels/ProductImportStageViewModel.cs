using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    class ProductImportStageViewModel : ImportStageViewModel, IStageViewModel
    {
        public ProductImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base(fastProductivityViewModel) { }
        public override string Header { get; } = "Номенклатура. Импорт.";
        public override int StageNum { get; } = 0;

        protected override void Open ( object obj )
        {
            var fileName = base.OpenExcelFile();


        }
    }
}
