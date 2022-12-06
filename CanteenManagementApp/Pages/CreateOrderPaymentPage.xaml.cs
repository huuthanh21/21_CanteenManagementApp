using CanteenManagementApp.MVVM.ViewModel;
using System.Text.RegularExpressions;
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
            InitializeComponent();
            CreateOrderVM = viewModel;
            DataContext = CreateOrderVM;

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
                PanelPaymentMethods.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetCorrespondingLayout();
        }

        private void TextboxInput_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
