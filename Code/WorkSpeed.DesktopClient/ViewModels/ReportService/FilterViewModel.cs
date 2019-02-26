using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agbm.Wpf.MvvmBaseLibrary;
using WorkSpeed.Data.Models;

namespace WorkSpeed.DesktopClient.ViewModels.ReportService
{
    public class FilterViewModel : ViewModel, IEntityObservableCollection< object >
    {
        private static readonly Func< object, string> TRUE_CAPTION;
        private readonly ObservableCollection< object > _entities;

        #region Ctor

        static FilterViewModel ()
        {
            TRUE_CAPTION = b => "Да";
        }

        private FilterViewModel ( string header )
        {
            Header = header;

            _entities = new ObservableCollection< object >();
            Entities = new ReadOnlyObservableCollection< object >( _entities );
        }

        public FilterViewModel ( string header, bool isCheckedValue )
            : this( header )
        {
            var boolItem = new FilterItemViewModel( isCheckedValue, TRUE_CAPTION );
            boolItem.PropertyChanged += OnFilterItemPropertyChanged;
            FilterItemVmCollection = new ObservableCollection< FilterItemViewModel >( new [] { boolItem });
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
        }

        #endregion


        #region Properties

        public string Header { get; }

        public ReadOnlyObservableCollection< object > Entities { get; set; }
        public ObservableCollection< FilterItemViewModel > FilterItemVmCollection  { get; private set; }

        #endregion


        #region Methods

        private void OnFilterItemPropertyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( nameof( FilterItemViewModel.IsChecked ) ) ) {  return; }

            if ( (( FilterItemViewModel )sender).IsChecked) {
                _entities.Add( (( FilterItemViewModel )sender).Entity );
            }
            else {
                _entities.Remove( (( FilterItemViewModel )sender).Entity );
            }
        }

        #endregion
    }
}
