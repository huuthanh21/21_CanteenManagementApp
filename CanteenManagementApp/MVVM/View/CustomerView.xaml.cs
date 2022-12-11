using CanteenManagementApp.MVVM.ViewModel;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        private CustomerViewModel CustomerViewModel { get; set; } = new CustomerViewModel(null);

        public CustomerView()
        {
            InitializeComponent();

            DataContext = CustomerViewModel;
        }
    }
}