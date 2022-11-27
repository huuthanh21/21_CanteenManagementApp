using System;
using System.Windows;
using System.Windows.Controls;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderPaymentPage.xaml
    /// </summary>
    public partial class CreateOrderPaymentPage : Page
    {
        public CreateOrderPaymentPage()
        {
            InitializeComponent();
        }

        private void ButtonNavigateReceiptPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CreateOrderReceiptPage.xaml", UriKind.Relative));
        }
    }
}
