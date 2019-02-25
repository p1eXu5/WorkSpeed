using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.Productivity
{
    public class ProductivityViewModel : ViewModel
    {
        protected const string SPEED_IN_LINES = "стр./ч";
        protected const string SPEED_IN_SCANS = "скан./ч";
        protected const string SPEED_IN_VOLUMES = "куб./ч";
        protected const string SPEED_IN_TIME = "часов";

        private double _speed;
        private string _speedLabeling;
        private string _annotation;
        protected Queue< AspectsViewModel> _queue;
        private AspectsViewModel _aspects;


        public ProductivityViewModel ( Operation operation )
        {
            Operation = operation;
            _queue = new Queue< AspectsViewModel >();
        }

        public Operation Operation { get; }
        public int OperationId => Operation.Id;

        public AspectsViewModel Aspects
        {
            get => _aspects;
            private set {
                _aspects = value;
                OnPropertyChanged();
            }
        }

        public double Speed
        {
            get => _speed;
            protected set {
                _speed = value;
                OnPropertyChanged();
            }
        }

        public string SpeedLabeling
        {
            get => _speedLabeling;
            protected set {
                _speedLabeling = value;
                OnPropertyChanged();
            }
        }

        public string Annotation
        {
            get => _annotation;
            set {
                _annotation = value;
                OnPropertyChanged();
            }
        }

        protected void Next ()
        {
            if ( _queue.Count == 0 ) return;
            Aspects = _queue.Dequeue();
            _queue.Enqueue( Aspects );
        }
    }
}
