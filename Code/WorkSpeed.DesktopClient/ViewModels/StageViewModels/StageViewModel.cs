﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.Interfaces;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public abstract class StageViewModel : ViewModel, IStageViewModel
    {
        protected readonly IFastProductivityViewModel FastProductivityViewModel;
        private bool _canMoveNext;

        protected StageViewModel ( IFastProductivityViewModel fastProductivityViewModel )
        {
            FastProductivityViewModel = fastProductivityViewModel ?? throw new ArgumentNullException();
            ForwardCommand = new MvvmCommand( Forward, CanForward );
        }

        public abstract string Header { get; }
        public abstract int StageNum { get; }

        public ICommand ForwardCommand { get; }
        public ICommand BackwardCommand => new MvvmCommand(Backward);
        public event EventHandler<EventArgs> MoveNextRequested;
        public event EventHandler<EventArgs> MoveBackRequested;

        public bool CanMoveNext
        {
            get => _canMoveNext;
            set {
                _canMoveNext = value;
                (( MvvmCommand )ForwardCommand ).RaiseCanExecuteChanged();
            }
        }

        

        protected virtual void Forward ( object obj )
        {
            MoveNextRequested?.Invoke( this, EventArgs.Empty );
        }

        protected virtual bool CanForward ( object obj ) => CanMoveNext;

        protected virtual void Backward ( object obj )
        {
            MoveBackRequested?.Invoke( this, EventArgs.Empty );
        }
    }
}
