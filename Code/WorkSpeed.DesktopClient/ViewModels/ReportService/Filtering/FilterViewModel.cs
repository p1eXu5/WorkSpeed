using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService.Filtering
{
    public class FilterViewModel : ViewModel
    {
        private static readonly Func< object, string> TRUE_CAPTION;
        private static readonly Func< object, string> FALSE_CAPTION;
        private readonly List< object > _entities;
        private readonly object _locker = new object();

        #region Ctor

        static FilterViewModel ()
        {
            TRUE_CAPTION = b => "Да";
            FALSE_CAPTION = b => "Нет";
        }

        private FilterViewModel ( string header )
        {
            Header = header;
            _entities = new List< object >();
        }

        public FilterViewModel ( string header, bool isCheckedValue )
            : this( header )
        {
            var trueItem = new FilterItemViewModel( true, TRUE_CAPTION );
            trueItem.PropertyChanged += OnFilterItemPropertyChanged;

            var falseItem = new FilterItemViewModel( false, FALSE_CAPTION );
            falseItem.PropertyChanged += OnFilterItemPropertyChanged;

            FilterItemVmCollection = new ObservableCollection< FilterItemViewModel >( new [] { trueItem, falseItem });

            trueItem.IsChecked = isCheckedValue;
            falseItem.IsChecked = isCheckedValue;
        }

        public FilterViewModel ( string header, IEnumerable< object > entities, Func< object, string> captionFunc )
            : this( header )
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities), @"entities cannot be null.");
            if (captionFunc == null) throw new ArgumentNullException(nameof(captionFunc), @"captionFunc cannot be null.");

            FilterItemVmCollection = new ObservableCollection< FilterItemViewModel >( 
                entities.Select(
                    e => {
                        var item = new FilterItemViewModel( e, captionFunc );
                        item.PropertyChanged += OnFilterItemPropertyChanged;
                        return item;
                    } ) 
            );

            foreach ( var filterItem in FilterItemVmCollection ) {
                filterItem.IsChecked = true;
            }
        }

        #endregion


        public event EventHandler< EventArgs > FilterChanged;


        #region Properties

        public string Header { get; }

        public IEnumerable< object > Entities => _entities;
        public ObservableCollection< FilterItemViewModel > FilterItemVmCollection  { get; private set; }

        #endregion


        #region Methods

        private void OnFilterItemPropertyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( nameof( FilterItemViewModel.IsChecked ) ) ) {  return; }

            lock ( _locker ) {

                if ( (( FilterItemViewModel )sender).IsChecked ) {
                    _entities.Add( (( FilterItemViewModel )sender).Entity );
                }
                else {
                    _entities.Remove( (( FilterItemViewModel )sender).Entity );
                }
            }

            OnFilterChanged();
        }

        private void OnFilterChanged ()
        {
            FilterChanged?.Invoke( this, EventArgs.Empty );
        }

        #endregion
    }
}
