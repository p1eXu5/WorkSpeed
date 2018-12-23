using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkSpeed.MvvmBaseLibrary;

namespace WorkSpeed.DesktopClient.ViewModels.StageViewModels
{
    public interface IStageViewModel
    {
        int StageNum { get; }
        string Header { get; }
        string Message { get; }
        
        ICommand ForwardCommand { get; }
        ICommand BackwardCommand { get; }

        event EventHandler< MoveRequestedEventArgs > MoveRequested;
    }
}
