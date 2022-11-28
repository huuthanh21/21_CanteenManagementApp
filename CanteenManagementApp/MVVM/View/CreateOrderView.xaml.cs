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
        public CreateOrderViewModel viewModel { get; set; }
        public CreateOrderView()
        {
            InitializeComponent();


            DataContext = viewModel;
        }
    }
}
