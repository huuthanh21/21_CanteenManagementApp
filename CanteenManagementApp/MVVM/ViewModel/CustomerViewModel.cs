using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System.Windows.Controls;
using System.Windows.Input;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CustomerViewModel : ObservableObject
    {
        public bool CustomerFound { get; set; } = false;
        public Customer Customer { get; set; }
        public ICommand FindCustomerCommand { get; set; }

        public CustomerViewModel()
        {
            FindCustomerCommand = new RelayCommand<TextBox>(tb => true, tb =>
            {
                var template = tb.Template;
                var control = (TextBox)template.FindName("TextboxInput", tb);
                if (!string.IsNullOrEmpty(control.Text))
                {
                    Customer = DbQueries.CustomerQueries.GetCustomerById(control.Text);
                    if (Customer != null)
                    {
                        CustomerFound = true;
                    }
                    else
                    {
                        CustomerFound = false;
                    }
                }
                else
                {
                    CustomerFound = false;
                }
            });


        }

    }
}
