using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderMainPage.xaml
    /// </summary>
    public partial class CreateOrderMainPage : Page
    {
        public CreateOrderMainPage()
        {
            InitializeComponent();
        }

        private void ButtonContinue_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CreateOrderPaymentPage.xaml", UriKind.Relative));   
        }
    }
}
