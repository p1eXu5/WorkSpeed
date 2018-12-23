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
    class ProductsImportStageViewModel : ImportStageViewModel, IStageViewModel
    {
        public ProductsImportStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum ) 
            : base( fastProductivityViewModel, stageNum )
        { }

        public override string Header { get; } = "Номенклатура. Импорт.";

        protected override async void Open ( object obj )
        {
            var fileName = base.OpenExcelFile();

            if ( String.IsNullOrWhiteSpace( fileName ) ) return;

            FileName = fileName;

            IsInProgress = true;
            ProgressCounter = 0;
            Message = "Загрузка...";

            var productsLastCount = Warehouse.GetProducts().Count();

            var cancellationToken = GetCancellationToken();
            var progress = new Progress< double >( (d) => ProgressCounter = d );

            bool areProductsAdded = await Warehouse.ImportAsync< ProductImportModel >( fileName, cancellationToken, progress );

            if ( areProductsAdded ) {

                Message = $"Добавлено { Warehouse.GetProducts().Count() - productsLastCount } SKU";
                UpdateCanForward();
            }
            else {

                IsInProgress = false;
                Message = "Файл не содержит позиций номенклатуры.";
            }
        }

        protected override bool CanForward ( object obj )
        {
            return Warehouse.GetProducts().Any();
        }
    }
}
