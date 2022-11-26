using CanteenManagementApp.Core;
using System;
using System.Windows.Navigation;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CreateOrderViewModel : ObservableObject
    {
        public RelayCommand NavigatePaymentPageCommand { get; set; }

        public CreateOrderViewModel()
        {
        }
    }
}
