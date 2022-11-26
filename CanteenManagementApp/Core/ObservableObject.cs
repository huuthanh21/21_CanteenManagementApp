using System.ComponentModel;

namespace CanteenManagementApp.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
