using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.Pages;
using System;
using System.Windows.Navigation;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CreateOrderViewModel : ObservableObject
    {
        public RelayCommand NavigateMainPageCommand { get; set; }
        public RelayCommand NavigatePaymentPageCommand { get; set; }
        public RelayCommand NavigateReceiptPageCommand { get; set; }

        private object _currentPage;

        public object CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public Customer Customer { get; set; } = null;
        public bool HasCustomer { get => Customer != null; set => HasCustomer = value; }

        public CreateOrderMainPage CreateOrderMainPage { get; set; }
        public CreateOrderPaymentPage CreateOrderPaymentPage { get; set; }
        public CreateOrderReceiptPage CreateOrderReceiptPage { get; set; }

        public CreateOrderViewModel(Customer customer = null)
        {
            CreateOrderMainPage = new CreateOrderMainPage(this);
            CreateOrderPaymentPage = new CreateOrderPaymentPage(this);
            CreateOrderReceiptPage = new CreateOrderReceiptPage(this);

            CurrentPage = CreateOrderMainPage;

            NavigateMainPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderMainPage;
            });

            NavigatePaymentPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderPaymentPage;
            });

            NavigateReceiptPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderReceiptPage;
            });
        }
    }
}
