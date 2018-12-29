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
        #region Fields

        private readonly TwoDirectionQueue<Func<IStageViewModel>> _stageQueue;
        private IStageViewModel _stageViewModel;
        private IStageViewModel _stageViewModel2;
        private readonly IWarehouse _warehouse;

        private int _stageIndex;

        #endregion


        #region Ctor

        public FastProductivityViewModel ( IWarehouse warehouse )
        {
            _warehouse = warehouse ?? throw new ArgumentNullException( nameof( warehouse ), @"IWarehouse cannot be null" );

            _stageQueue = new TwoDirectionQueue< Func< IStageViewModel >>();

            //_stageQueue.Enqueue( () => new ShiftSetupStageViewModel( this, 0 ) );
            //_stageQueue.Enqueue( () => new CategoriesThresholdStageViewModel( this, 1 ) );
            _stageQueue.Enqueue( () => new ProductsImportStageViewModel( this, 2 ) );
            _stageQueue.Enqueue( () => new EmployeesImportStageViewModel( this, 3 ) );
            //_stageQueue.Enqueue( () => new CheckEmployeesStageViewModel( this, 4 ) );
            _stageQueue.Enqueue( () => new ActionsImportStageViewModel( this, 5 ) );
            //_stageQueue.Enqueue( () => new ProductivityStageViewModel( this, 6 ) );

            SetStageViewModel( GetNextStageViewModel( _stageQueue ) );
        }

        #endregion


        #region Properties

        public IWarehouse Warehouse => _warehouse;

        public IStageViewModel StageViewModel
        {
            get => _stageViewModel;
            set {
                _stageViewModel = value;
                if ( _stageViewModel != null ) StageIndex = _stageViewModel.StageNum;
                OnPropertyChanged();
            }
        }

        public IStageViewModel StageViewModel2
        {
            get => _stageViewModel2;
            set {
                _stageViewModel2 = value;
                if ( _stageViewModel2 != null ) StageIndex = _stageViewModel2.StageNum;
                OnPropertyChanged();
            }
        }

        public int StageIndex
        {
            get => _stageIndex;
            set {
                _stageIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Methods

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

        private IStageViewModel GetNextStageViewModel ( TwoDirectionQueue< Func<IStageViewModel>> queue )
        {
            var stageViewModel = queue.Dequeue()();

            stageViewModel.MoveRequested += OnMoveRequestedEventHandler;
            return stageViewModel;

        }

        private void OnMoveRequestedEventHandler ( object sender, MoveRequestedEventArgs args )
        {
            var stageViewModel = (sender as IStageViewModel) ?? throw new ArgumentException();
            stageViewModel.MoveRequested -= OnMoveRequestedEventHandler;

            if ( sender.GetType() == typeof( ProductsImportStageViewModel ) ) {

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

        #endregion
    }
}
