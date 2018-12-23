using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NpoiExcel;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class FastProductivityViewModel : ViewModel, IFastProductivityViewModel
    {
        private IStageViewModel _stageViewModel;
        private IStageViewModel _stageViewModel2;
        private readonly Warehouse _warehouse = new Warehouse( new RuntimeWorkSpeedBusinessContext(), new ExcelDataImporter() );

        public FastProductivityViewModel ()
        {
            var queue = new Queue< IStageViewModel >(new IStageViewModel[] {

                new ProductImportStageViewModel( _warehouse ),
                new EmployeeImportStageViewModel( _warehouse ), 
                new CheckEmployeesStageViewModel( _warehouse ),
                new ProductivityStageViewModel( _warehouse ), 
            });

            SetStageViewModel( GetStageViewModel( queue ) );
        }

        private void SetStageViewModel ( IStageViewModel newStageViewModel )
        {
            if ( newStageViewModel.StageNum % 2 == 0 ) {

                StageViewModel2 = null;
                StageViewModel = newStageViewModel;
            }
            else {
                StageViewModel = null;
                StageViewModel2 = newStageViewModel;
            }
        }

        public IStageViewModel StageViewModel
        {
            get => _stageViewModel;
            set {
                _stageViewModel = value;
                if ( _stageViewModel != null ) OnPropertyChanged( nameof( StageIndex ) );
                OnPropertyChanged();
            }
        }

        public IStageViewModel StageViewModel2
        {
            get => _stageViewModel2;
            set {
                _stageViewModel2 = value;
                if ( _stageViewModel2 != null ) OnPropertyChanged( nameof( StageIndex ) );
                OnPropertyChanged();
            }
        }

        public int StageIndex => StageViewModel?.StageNum ?? StageViewModel2.StageNum;

        public IWarehouse Warehouse => _warehouse;

        public bool CheckStage ( IStageViewModel stageViewModel )
        {
            throw new NotImplementedException();
        }

        private IStageViewModel GetStageViewModel ( Queue< IStageViewModel > queue )
        {
            var stageViewModel = queue.Dequeue();

            stageViewModel.MoveNextRequested += OnMoveNextEventHandler;
            return stageViewModel;

            void OnMoveNextEventHandler ( object e2, EventArgs a2 )
            {
                stageViewModel.MoveNextRequested -= OnMoveNextEventHandler;
                SetStageViewModel( GetStageViewModel( queue ) );
            }
        }
    }
}
