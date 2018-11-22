using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkSpeed.MvvmBaseLibrary.Annotations;

namespace WorkSpeed.MvvmBaseLibrary
{
    public abstract class NotifyingObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
