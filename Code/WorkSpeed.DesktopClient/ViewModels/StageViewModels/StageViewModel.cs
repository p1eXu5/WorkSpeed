﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using WorkSpeed.Interfaces;
//using WorkSpeed.MvvmBaseLibrary;

//namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
//{
//    public abstract class StageViewModel : ViewModel, IStageViewModel
//    {
//        #region Fields

//        protected readonly IFastProductivityViewModel FastProductivityViewModel;
//        protected bool _canMoveNext;
//        protected string _message;

//        #endregion


//        #region Ctor

//        protected StageViewModel ( IFastProductivityViewModel fastProductivityViewModel, int stageNum )
//        {
//            FastProductivityViewModel = fastProductivityViewModel ?? throw new ArgumentNullException();
//            ForwardCommand = new MvvmCommand( Forward, CanForward );

//            StageNum = stageNum;
//        }

//        #endregion


//        #region Properties

//        public abstract string Header { get; }
//        public int StageNum { get; }

//        public string Message
//        {
//            get => _message;
//            protected set {
//                _message = value;
//                OnPropertyChanged();
//            }
//        }

//        public ICommand ForwardCommand { get; }
//        public ICommand BackwardCommand => new MvvmCommand(Backward);
//        public event EventHandler< MoveRequestedEventArgs > MoveRequested;

//        public bool CanMoveNext
//        {
//            get => _canMoveNext;
//            set {
//                _canMoveNext = value;
//                (( MvvmCommand )ForwardCommand ).RaiseCanExecuteChanged();
//            }
//        }

//        protected IWarehouse Warehouse => FastProductivityViewModel.Warehouse;

//        #endregion


//        #region Methods

//        protected virtual void Forward ( object obj )
//        {
//            MoveRequested?.Invoke( this, new MoveRequestedEventArgs( Directions.Forward ) );
//        }

//        protected virtual bool CanForward ( object obj ) => CanMoveNext;

//        protected void UpdateCanForward ()
//        {
//            ((MvvmCommand)ForwardCommand).RaiseCanExecuteChanged();
//        }

//        protected virtual void Backward ( object obj )
//        {
//            MoveRequested?.Invoke( this, new MoveRequestedEventArgs( Directions.Backward ) );
//        }

//        #endregion
//    }
//}
