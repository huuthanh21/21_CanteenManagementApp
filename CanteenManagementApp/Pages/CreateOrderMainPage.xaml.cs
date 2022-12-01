using CanteenManagementApp.MVVM.ViewModel;
using System.Windows.Controls;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderMainPage.xaml
    /// </summary>
    public partial class CreateOrderMainPage : Page
    {
        public CreateOrderViewModel CreateOrderVM { get; set; }

        public CreateOrderMainPage(CreateOrderViewModel viewModel)
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
            }
        }

    }
}
