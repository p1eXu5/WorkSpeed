using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels
{
    public interface IStageViewModel
    {
        string Header { get; }
        int StageNum { get; }
        ICommand OpenCommand { get; }
        ICommand ForwardCommand { get; }
        ICommand BackwardCommand { get; }

        event EventHandler< EventArgs > MoveNextRequested;
        event EventHandler< EventArgs > MoveBackRequested;
    }
}
