using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VendingMachineDataInjectionInterfaces
{
    public class MVVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
