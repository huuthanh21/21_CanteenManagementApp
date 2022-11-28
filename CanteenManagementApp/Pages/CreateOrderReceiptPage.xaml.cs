using CanteenManagementApp.MVVM.ViewModel;
using System.Windows.Controls;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderReceiptPage.xaml
    /// </summary>
    public partial class CreateOrderReceiptPage : Page
    {
        public CreateOrderViewModel CreateOrderVM { get; set; }

        public CreateOrderReceiptPage(CreateOrderViewModel viewModel)
        {
            CreateOrderVM = viewModel;

            InitializeComponent();
        }

        private void ButtonNavigateMainPage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM.NavigateMainPageCommand.Execute(null);
        }
    }
}
