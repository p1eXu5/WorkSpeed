using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;
using WorkSpeed.ProductivityCalculator;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class ProductivityStageViewModel : StageViewModel
    {
        private readonly ObservableCollection< ProductivityViewModel > _productivities;
        private Progress< Productivity > _progress;
        private double _progressCounter;
        private double _progressStep;

        public ProductivityStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
            : base( fastProductivityViewModel, stageNum )
        {
            _productivities = new ObservableCollection< ProductivityViewModel >();
            Productivities = new ReadOnlyObservableCollection< ProductivityViewModel >( _productivities );

            _progress = new Progress< Productivity >( OnProgress );
        }

        public override string Header { get; } = "Выработка.";

        public ReadOnlyObservableCollection< ProductivityViewModel > Productivities { get; }

        public double ProgressCounter
        {
            get => _progressCounter;
            set {
                _progressCounter = value;
                OnPropertyChanged();
            }
        }


        public ICommand GenerateCommand => new MvvmCommand( Generate );

        private async void Generate ( object obj )
        {
            _progressCounter = 0.0;

            var employeeCount = Warehouse.GetGatheringActions().Select( a => a.Employee ).Distinct().Count();

            if (employeeCount == 0) return;

            _progressStep = 1.0 / employeeCount;

            await Warehouse.GetProductivitiesAsync( _progress );
        }

        private void OnProgress ( Productivity productivity )
        {
            ProgressCounter = _progressCounter + _progressStep;

            _productivities.Add( new ProductivityViewModel( productivity ) );
        }
    }
}
