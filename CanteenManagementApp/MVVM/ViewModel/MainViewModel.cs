using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand CreateOrderViewCommand { get; set; }
        public RelayCommand CreateOrderViewWithCustomerCommand { get; set; }
        public RelayCommand CreateOrderViewWithTopUp { get; set; }
        public RelayCommand CustomerViewCommand { get; set; }
        public RelayCommand MenuViewCommand { get; set; }
        public RelayCommand StorageViewCommand { get; set; }
        public RelayCommand ReportViewCommand { get; set; }

        public CreateOrderViewModel CreateOrderVM { get; set; }
        public CreateOrderViewModel CreateOrderWithCustomerVM { get; set; }
        public CustomerViewModel CustomerVM { get; set; }
        public MenuViewModel MenuVM { get; set; }
        public StorageViewModel StorageVM { get; set; }
        public ReportViewModel ReportVM { get; set; }

        public object CurrentView { get; set; }

        public MainViewModel()
        {
            CreateOrderVM = new CreateOrderViewModel();
            CreateOrderWithCustomerVM = new CreateOrderViewModel();
            CustomerVM = new CustomerViewModel(this);
            MenuVM = new MenuViewModel();
            StorageVM = new StorageViewModel();
            ReportVM = new ReportViewModel();

            CurrentView = CreateOrderVM;

            CreateOrderViewCommand = new RelayCommand(o =>
            {
                CreateOrderViewModel.Customer = null;
                CurrentView = CreateOrderVM;
            });

            CreateOrderViewWithCustomerCommand = new RelayCommand(o =>
            {
                CreateOrderViewModel.Customer = (Customer)o;
                CreateOrderViewModel.TopUpAmount = 0;

                CurrentView = CreateOrderWithCustomerVM;
            });

            // parameter: Tuple<Customer, int>
            CreateOrderViewWithTopUp = new RelayCommand(tuple =>
            {
                CreateOrderViewModel.Customer = (tuple as Tuple<Customer, int>).Item1;
                CreateOrderViewModel.TopUpAmount = (tuple as Tuple<Customer, int>).Item2;
                CurrentView = CreateOrderWithCustomerVM;
            });

            CustomerViewCommand = new RelayCommand(o =>
            {
                CustomerViewModel.MainViewModel = this;
                CurrentView = CustomerVM;
            });

            MenuViewCommand = new RelayCommand(o =>
            {
                CurrentView = MenuVM;
            });

            StorageViewCommand = new RelayCommand(o =>
            {
                CurrentView = StorageVM;
            });

            ReportViewCommand = new RelayCommand(o =>
            {
                CurrentView = new ReportViewModel();
            });
        }
    }
}