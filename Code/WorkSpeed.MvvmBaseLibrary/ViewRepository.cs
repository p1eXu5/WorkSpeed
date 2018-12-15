using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WorkSpeed.MvvmBaseLibrary
{
    public class ViewRepository : IDialogRepository
    {
        private readonly Dictionary<Type, Type> _repository;
        private readonly Window _owner;

        public ViewRepository( Window owner )
        {
            _repository = new Dictionary<Type, Type>();
        }

        public void Register< TViewModel, TView >() 
            where      TView : IDialog, new()
            where TViewModel : IDialogCloseRequested
        {
            _repository[ typeof( TViewModel ) ] = typeof( TView );
        }

        public IDialog GetView< TViewModel >( TViewModel viewModel )
            where TViewModel : IDialogCloseRequested
        {
            if ( _repository.TryGetValue( typeof( TViewModel ), out var viewType ) ) {

                IDialog view = ( IDialog )Activator.CreateInstance( viewType );

                view.DataContext = viewModel;
                view.Owner = _owner;

                EventHandler<CloseRequestedEventArgs> onCloseDialogHandler = null;

                onCloseDialogHandler = ( s, e ) =>
                {
                    viewModel.CloseRequested -= onCloseDialogHandler;

                    (( IDialog )s ).DialogResult = e.DialogResult;
                };

                return view;
            }

            return null;
        }
    }
}
