using CanteenManagementApp.Core;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CreateOrderViewModel : ObservableObject
    {
        public RelayCommand PaymentCommand { get; set; }
        public RelayCommand ReceiptCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public CreateOrderViewModel()
        {

        }
    }
}
