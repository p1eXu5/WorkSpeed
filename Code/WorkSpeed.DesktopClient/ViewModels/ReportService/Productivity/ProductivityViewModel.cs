using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Productivity
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
        private IEnumerable< (double val, string ann) > _selectedAspects;


        public ProductivityViewModel ( Operation operation )
        {
            Operation = operation;
            _queue = new Queue< AspectsViewModel >();
        }

        public Operation Operation { get; }
        public int OperationId => Operation.Id;

        public IEnumerable< (double val, string ann) > SelectedAspects
        {
            get => _selectedAspects;
            private set {
                _selectedAspects = value;
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

        /// <summary>
        ///     Sets next aspect collection.
        /// </summary>
        protected void Next ()
        {
            if ( _queue.Count == 0 ) return;
            var next = _queue.Dequeue();
            SelectedAspects = next.Aspects;
            Annotation = next.Annotation;
            _queue.Enqueue( next );
        }
    }
}
