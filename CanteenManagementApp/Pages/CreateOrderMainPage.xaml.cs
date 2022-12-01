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
            if (CreateOrderVM.HasCustomer)
            {
                CustomerId.Content = CreateOrderVM.Customer.Id;
            }
            else
            {
                CustomerId.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ButtonContinue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM?.NavigatePaymentPageCommand.Execute(null);
        }

        private void ButtonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM?.NavigateMainPageCommand.Execute(null);
        }

        private void Decrease_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int indexSelected = foodListView.SelectedIndex;
            if(indexSelected != -1)
            {
                if (CreateOrderVM._ListFoodItemOrder[indexSelected]._amount > 0)
                {
                    CreateOrderVM._ListFoodItemOrder[indexSelected]._amount--;
                }
            }
            CreateOrderVM.UpdateTotalOrder();

        }

        private void Increase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int indexSelected = foodListView.SelectedIndex;
            if (indexSelected != -1)
                CreateOrderVM._ListFoodItemOrder[indexSelected]._amount++;

            CreateOrderVM.UpdateTotalOrder();
        }
    }
}
