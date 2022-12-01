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
            if (CreateOrderVM.HasCustomer)
            {
                CustomerId.Content = CreateOrderVM.Customer.Id;
            }
            else
            {
                CustomerId.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
