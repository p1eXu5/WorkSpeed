using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Agbm.Wpf.MvvmBaseLibrary;

namespace Experiments.Wpf.ViewModels
{
    public class ListBoxItemsSourceDynamicChangeViewModel : ViewModel
    {
        private IEnumerable< object > _collection;
        private readonly ObservableCollection< object > _collection1;
        private readonly ObservableCollection< object > _collection2;

        public ListBoxItemsSourceDynamicChangeViewModel ()
        {
            _collection1 = new ObservableCollection< object >( new [] {
                "one",
                "two"
            });

             _collection2 = new ObservableCollection< object >( new [] {
                "twenty",
                "thirty"
            });

            Command = new MvvmCommand( ( o ) =>
                                      {
                                          if ( Collection == _collection1 ) {
                                              Collection = _collection2;
                                          }
                                          else {
                                              Collection = _collection1;
                                          }
                                      } );

            Collection = _collection1;
        }

        public IEnumerable< object > Collection
        {
            get => _collection;
            set {
                _collection = value;
                OnPropertyChanged();
            }
        }

        public ICommand Command { get; }
    }
}
