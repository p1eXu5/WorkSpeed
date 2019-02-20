using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using CircleDiagramTest.Models;

namespace CircleDiagramTest.ViewModels
{
    public class ProductivityViewModel : ViewModel
    {
        private double _speed;
        private string _speedLabeling;
        private string _annotation;

        private readonly Operation _operation;

        public ProductivityViewModel ( Operation operation )
        {
            _operation = operation;
        }

        public int OperationId => _operation.Id;
        public IEnumerable< (double val, string ann) > Aspects { get; set; }

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
    }
}
