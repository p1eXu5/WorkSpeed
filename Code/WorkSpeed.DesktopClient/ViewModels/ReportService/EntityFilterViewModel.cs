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
    public class EntityFilterViewModel< T > : ViewModel, IEntityObservableCollection< T >
    {
        private static readonly Func< T, string> TRUE_CAPTION;
        private readonly ObservableCollection< T > _entities;

        static EntityFilterViewModel ()
        {
            TRUE_CAPTION = b => "Да";
        }

        public EntityFilterViewModel ( string header, IEnumerable< T > entities, Func< T, string> caption )
        {
            if ( typeof( T ).IsAssignableFrom( typeof( bool ) ) ) {

                var boolItem = new FilterItemViewModel< T >( entities.First(), TRUE_CAPTION );
                boolItem.PropertyChanged += OnFilterItemPropertyChanged;
                FilterItemVmCollection = new ObservableCollection< FilterItemViewModel< T > >( new [] { boolItem });
            }
            else {
                FilterItemVmCollection = new ObservableCollection< FilterItemViewModel< T > >( entities.Select(
                    e => {
                        var item = new FilterItemViewModel< T >( e, caption );
                        item.PropertyChanged += OnFilterItemPropertyChanged;
                        return item;
                    } ) 
                );
            }

            _entities = new ObservableCollection< T >();
            Entities = new ReadOnlyObservableCollection< T >( _entities );
        }

        public ReadOnlyObservableCollection< T > Entities { get; set; }

        public ObservableCollection< FilterItemViewModel< T > > FilterItemVmCollection  { get; private set; }

        private void OnFilterItemPropertyChanged ( object sender, PropertyChangedEventArgs args )
        {
            if ( !args.PropertyName.Equals( nameof( FilterItemViewModel<T>.IsChecked ) ) ) {  return; }

            if ( (( FilterItemViewModel< T > )sender).IsChecked) {
                _entities.Add( (( FilterItemViewModel< T > )sender).Entity );
            }
            else {
                _entities.Remove( (( FilterItemViewModel< T > )sender).Entity );
            }
        }
    }
}
