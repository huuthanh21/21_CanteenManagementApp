using CanteenManagementApp.MVVM.ViewModel;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        CustomerViewModel CustomerViewModel { get; set; } = new CustomerViewModel(null);
        public CustomerView()
        {   
            InitializeComponent();

            DataContext = CustomerViewModel;
        }

        private void billListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBlock_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
