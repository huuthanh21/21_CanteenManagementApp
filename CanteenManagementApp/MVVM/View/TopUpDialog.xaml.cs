using CanteenManagementApp.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for TopUpDialog.xaml
    /// </summary>
    public partial class TopUpDialog : Window
    {
        public int TopUpAmount { get; set; }
        public CreateOrderViewModel CreateOrderViewModel { get; set; }
        public CustomerViewModel CustomerViewModel { get; set; }

        public TopUpDialog(CreateOrderViewModel createOrderViewModel)
        {
            InitializeComponent();
            CreateOrderViewModel = createOrderViewModel;
            DataContext = CreateOrderViewModel;
        }

        public TopUpDialog(CustomerViewModel customerViewModel)
        {
            InitializeComponent();
            CustomerViewModel = customerViewModel;
            DataContext = CustomerViewModel;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            int input = int.Parse((TextboxTopUp.Template.FindName("TextboxInput", TextboxTopUp) as TextBox).Text);
            if (input % 10000 != 0)
            {
                MessageBox.Show("Số tiền nạp phải là bội số của 10.000", "Sai giá trị nạp", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            TopUpAmount = int.Parse((TextboxTopUp.Template.FindName("TextboxInput", TextboxTopUp) as TextBox).Text) / 10000;
            DialogResult = true;
        }
    }
}