
using Agbm.Wpf.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.Dialogs
{
    public class ErrorViewModel : DialogViewModel
    {
        public ErrorViewModel ()
        { }

        public ErrorViewModel ( string message )
        {
            Message = message;
        }

        public string Message { get; }
    }
}
