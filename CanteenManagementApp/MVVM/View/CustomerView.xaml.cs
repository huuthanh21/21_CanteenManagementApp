using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.ViewModel;
using System;
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

        private void ListViewItem_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is not ListViewItem item) return;

            var receipt = item.DataContext as Receipt;

            CustomerViewModel.ViewReceiptDetailsCommand.Execute(receipt.Id);
        }
    }
}