using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.FileModels;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    class ProductImportStageViewModel : ImportStageViewModel, IStageViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public ProductImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) :
            base( fastProductivityViewModel )
        { }


        public override int StageNum { get; } = 0;
        public override string Header { get; } = "Номенклатура. Импорт.";

        public override string Message { get; protected set; } = "xcxc";


        protected override async void Open ( object obj )
        {
            var fileName = base.OpenExcelFile();

            if ( String.IsNullOrWhiteSpace( fileName ) ) return;

            var productsLastCount = Warehouse.Products.Count();

            _cancellationTokenSource.Cancel();
            var cancellationToken = _cancellationTokenSource.Token;
            var progress = new Progress< double >( (d) => ProgressCounter = d );

            bool areProductsAdded = await Warehouse.ImportAsync< ProductImportModel >( fileName, cancellationToken, progress );

            if ( areProductsAdded ) {

                Message = $"Было добавлено { Warehouse.Products.Count() - productsLastCount } SKU";
                UpdateCanForward();
            }
        }
    }
}
