using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.ViewModel;
using System.Windows.Controls;
using System.Windows.Input;

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
    }
}
