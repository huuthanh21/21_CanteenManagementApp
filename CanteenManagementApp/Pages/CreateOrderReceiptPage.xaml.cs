using CanteenManagementApp.MVVM.ViewModel;
using System.Windows;
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
            InitializeComponent();
            CreateOrderVM = viewModel;
            DataContext = CreateOrderVM;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetCorrespondingLayout();
        }

        private void SetCorrespondingLayout()
        {
            if (CreateOrderViewModel.HasCustomer)
            {
                CustomerId.Content = CreateOrderViewModel.Customer.Id;
            }
            else
            {
                CustomerId.Visibility = System.Windows.Visibility.Collapsed;
                PanelCustomerId.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(createOrderPage, "Receipt Printing");
                this.IsEnabled = true;
            }
        }
    }
}
