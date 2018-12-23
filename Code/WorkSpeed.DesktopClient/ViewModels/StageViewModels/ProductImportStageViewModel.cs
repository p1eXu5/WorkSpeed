using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    class ProductImportStageViewModel : ImportStageViewModel, IStageViewModel
    {
        public ProductImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) : base(fastProductivityViewModel) { }


        public override int StageNum { get; } = 0;
        public override string Header { get; } = "Номенклатура. Импорт.";
        public override string Message { get; protected set; } = "";


        protected override async void Open ( object obj )
        {
            var fileName = base.OpenExcelFile();

            if ( String.IsNullOrWhiteSpace( fileName ) ) return;

            var productsLastCount = Warehouse.Products.Count();
            bool areProductsAdded = await Warehouse.ImportAsync< ProductImportModel >( fileName );

            if ( areProductsAdded ) {

                Message = $"Было добавлено { Warehouse.Products.Count() - productsLastCount } SKU";
                UpdateCanForward();
            }
        }
    }
}
