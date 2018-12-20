using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NpoiExcel;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class FastProductivityViewModel : ViewModel
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

            _stageViewModel = SetStageViewModel( queue );
        }

        private IStageViewModel SetStageViewModel ( Queue< IStageViewModel > queue )
        {
            var stageViewModel = queue.Dequeue();

            EventHandler< EventArgs > moveNextEventHandler = null;
            moveNextEventHandler += ( e2, a2 ) =>
                                               {
                                                   stageViewModel.MoveNextRequested -= moveNextEventHandler;
                                                   var newStageViewModel = SetStageViewModel( queue );
                                                   if ( newStageViewModel.StageNum % 2 == 0 ) {
                                                       StageViewModel2 = null;
                                                       StageViewModel = newStageViewModel;
                                                   }
                                                   else {
                                                       StageViewModel = null;
                                                       StageViewModel2 = newStageViewModel;
                                                   }
                                               };
            stageViewModel.MoveNextRequested += moveNextEventHandler;

            return stageViewModel;
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
    }
}
