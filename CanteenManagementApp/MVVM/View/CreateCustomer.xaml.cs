using System.Windows;
using CanteenManagementApp.MVVM.Model;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for  CreateCustomer.xaml
    /// </summary>
    public partial class CreateCustomer : Window
    {
        public CreateCustomer()
        {
            InitializeComponent();
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string customerType = GetCheckedRadioButton();
            string customerId = TextBoxId.Text;
            string customerName = TextBoxName.Text;

            if (string.IsNullOrWhiteSpace(customerType) || string.IsNullOrWhiteSpace(customerId) || string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("Có trường còn trống, xin hãy kiểm tra lại.", "Lưu lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Close();
                await DbQueries.CustomerQueries.InsertCustomerAsync(customerId, customerName, customerType);
            }
        }

        private string GetCheckedRadioButton()
        {
            if ((bool)RadioButtonSV.IsChecked)
            {
                return RadioButtonSV.Content.ToString();
            }
            if ((bool)RadioButtonGV.IsChecked)
            {
                return RadioButtonGV.Content.ToString();
            }
            return RadioButtonCBNV.Content.ToString();
        }
    }
}
