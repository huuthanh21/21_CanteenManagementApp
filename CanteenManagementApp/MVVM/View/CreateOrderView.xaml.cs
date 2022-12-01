using CanteenManagementApp.MVVM.Model;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for CreateOrderView.xaml
    /// </summary>
    public partial class CreateOrderView : UserControl
    {
        public Customer Customer { get; set; } = null;

        public CreateOrderView()
        {
            InitializeComponent();
        }

        public CreateOrderView(Customer customer = null)
        {
            Customer = customer;

            InitializeComponent();
        }

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }
    }
}
