using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkSpeed.MvvmBaseLibrary
{
    public interface IDialogCloseRequested
    {
        ICommand OkCommand { get; }
        ICommand CanselCommand { get; }

        event EventHandler< CloseRequestedEventArgs > CloseRequested;
    }
}
