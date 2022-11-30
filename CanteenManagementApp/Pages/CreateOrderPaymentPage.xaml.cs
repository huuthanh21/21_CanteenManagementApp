﻿using CanteenManagementApp.MVVM.ViewModel;
using System.Windows.Controls;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderPaymentPage.xaml
    /// </summary>
    public partial class CreateOrderPaymentPage : Page
    {
        public CreateOrderViewModel CreateOrderVM { get; set; }

        public CreateOrderPaymentPage(CreateOrderViewModel viewModel)
        {
            CreateOrderVM = viewModel;

            InitializeComponent();
        }

        private void SetCorrespondingLayout()
        {
            if (CreateOrderVM.HasCustomer)
            {
                CustomerId.Content = CreateOrderVM.Customer.Id;
            }
            else
            {
                CustomerId.Visibility = System.Windows.Visibility.Collapsed;
                PanelPaymentMethods.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetCorrespondingLayout();
        }

        private void ButtonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM.NavigateMainPageCommand.Execute(null);
        }

        private void ButtonNavigateReceiptPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM.NavigateReceiptPageCommand.Execute(null);
        }
    }
}
