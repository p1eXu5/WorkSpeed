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
        public ProductImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel ) 
            : base( fastProductivityViewModel )
        { }

        public override int StageNum { get; } = 0;
        public override string Header { get; } = "Номенклатура. Импорт.";

        protected override async void Open ( object obj )
        {
            var fileName = base.OpenExcelFile();

            if ( String.IsNullOrWhiteSpace( fileName ) ) return;

            FileName = fileName;

            IsInProgress = true;
            ProgressCounter = 0;

            var productsLastCount = Warehouse.Products.Count();

            var cancellationToken = GetCancellationToken();
            var progress = new Progress< double >( (d) => ProgressCounter = d );

            bool areProductsAdded = await Warehouse.ImportAsync< ProductImportModel >( fileName, cancellationToken, progress );

            if ( areProductsAdded ) {

                Message = $"Добавлено { Warehouse.Products.Count() - productsLastCount } SKU";
                UpdateCanForward();
            }
        }

        protected override bool CanForward ( object obj )
        {
            return Warehouse.Products.Any();
        }
    }
}
