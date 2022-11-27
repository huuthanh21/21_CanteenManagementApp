using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderReceiptPage.xaml
    /// </summary>
    public partial class CreateOrderReceiptPage : Page
    {
        public CreateOrderReceiptPage()
        {
            InitializeComponent();
        }

        private void ButtonNavigateMainPage_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CreateOrderMainPage.xaml", UriKind.Relative));
        }
    }
}
