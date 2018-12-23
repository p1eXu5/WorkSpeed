using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Helpers;
using NpoiExcel;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.DesktopClient.ViewModels.StageViewModels;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public class FastProductivityViewModel : ViewModel, IFastProductivityViewModel
    {
        private readonly TwoDirectionQueue< IStageViewModel > _stageQueue;
        private IStageViewModel _stageViewModel;
        private IStageViewModel _stageViewModel2;
        private readonly Warehouse _warehouse = new Warehouse( new RuntimeWorkSpeedBusinessContext(), new ExcelDataImporter() );

        public FastProductivityViewModel ()
        {
            _stageQueue = new TwoDirectionQueue< IStageViewModel >();

            _stageQueue.Enqueue( new ProductImportStageViewModel( this ) );
            _stageQueue.Enqueue( new EmployeeImportStageViewModel( this ) );
            _stageQueue.Enqueue( new CheckEmployeesStageViewModel( this ) );
            _stageQueue.Enqueue( new ProductivityStageViewModel( this ) );

            SetStageViewModel( GetNextStageViewModel( _stageQueue ) );
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

        private void SetStageViewModel ( IStageViewModel newStageViewModel )
        {
            if ( newStageViewModel.StageNum % 2 == 0 ) {

                StageViewModel = newStageViewModel;
            }
            else {
                StageViewModel2 = newStageViewModel;
            }
        }

        private IStageViewModel GetNextStageViewModel ( TwoDirectionQueue< IStageViewModel > queue )
        {
            var stageViewModel = queue.Dequeue();

            stageViewModel.MoveRequested += OnMoveRequestedEventHandler;
            return stageViewModel;

        }

        private void OnMoveRequestedEventHandler ( object sender, MoveRequestedEventArgs args )
        {
            var stageViewModel = (sender as IStageViewModel) ?? throw new ArgumentException();
            stageViewModel.MoveRequested -= OnMoveRequestedEventHandler;

            if ( sender.GetType() == typeof( ProductImportStageViewModel ) ) {

                if ( args.Direction < 0 ) {
                    Exit( null );
                }
            }

            SetStageViewModel( GetNextStageViewModel( _stageQueue ) );
        }

        private void Exit ( object obj )
        {
            Application.Current.Shutdown();
        }
    }
}
