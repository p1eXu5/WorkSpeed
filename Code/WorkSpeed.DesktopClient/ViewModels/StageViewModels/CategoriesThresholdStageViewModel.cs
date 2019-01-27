//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Runtime;
//using System.Text;
//using System.Threading.Tasks;
//using WorkSpeed.DesktopClient.ViewModels.EntityViewModels;

//namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
//{
//    public class CategoriesThresholdStageViewModel : StageViewModel
//    {
//        private TimeSpan _threshold;

//        public CategoriesThresholdStageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
//            : base( fastProductivityViewModel, stageNum )
//        {
//            Categories = new ObservableCollection< CategoryViewModel >( Warehouse.GetCategories().Select( c => new CategoryViewModel( c ) ) );
//            _threshold = Warehouse.GetThreshold();

//            CanMoveNext = true;
//        }

//        public override string Header { get; } = "Настройка категорий и порогов";

//        public ObservableCollection< CategoryViewModel > Categories { get; set; }

//        public TimeSpan Threshold
//        {
//            get => _threshold;
//            set {
//                _threshold = value;
//                OnPropertyChanged();
//            }
//        }

//        protected override void Forward ( object obj )
//        {
//            base.Forward( obj );
//        }
//    }
//}
