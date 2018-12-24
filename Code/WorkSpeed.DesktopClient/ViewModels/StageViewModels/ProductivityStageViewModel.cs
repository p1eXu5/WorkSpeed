using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;
using WorkSpeed.Interfaces;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public class ProductivityStageViewModel : StageViewModel
    {
        private readonly ObservableCollection< ProductivityViewModel > _productivities;

        public ProductivityStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
            : base( fastProductivityViewModel, stageNum )
        {
            _productivities = new ObservableCollection< ProductivityViewModel >( Warehouse.GetEmployees().Select( e => Warehouse.GetProductivity( e ) ).Where( p => p != null ) );
            Productivities = new ReadOnlyObservableCollection< ProductivityViewModel >( _productivities );
        }

        public override string Header { get; } = "Выработка.";

        public ReadOnlyObservableCollection< ProductivityViewModel > Productivities { get; }

    }
}
