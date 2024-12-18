using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NeliasBookManager.presentation.Viewmodel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
            public event PropertyChangedEventHandler? PropertyChanged;

            public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
    }
}

