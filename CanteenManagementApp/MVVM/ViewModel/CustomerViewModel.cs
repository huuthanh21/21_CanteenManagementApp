using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CustomerViewModel : ObservableObject
    {
        public static MainViewModel MainViewModel { get; set; }
        public bool CustomerFound { get; set; } = false;
        public Customer Customer { get; set; }

        public bool IsShowing { get; set; } = true;
        public ICommand FindCustomerCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }
        public RelayCommand CreateOrderCommand { get; set; }
        public RelayCommand PasswordStateCommand { get; set; }

        public ObservableCollection<Item> recentItems;

        private readonly CollectionViewSource RecentItemsCollection;

        public ICollectionView RecentItemsSourceCollection => RecentItemsCollection.View;

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

            AddCustomerCommand = new RelayCommand(o =>
            {
                var screen = new CreateCustomer();
                screen.Show();
            });

            PasswordStateCommand = new RelayCommand(o =>
            {
                IsShowing = !IsShowing;
            });

            CreateOrderCommand = new RelayCommand(s =>
            {
                if (CustomerFound)
                {
                    MainViewModel.CreateOrderViewWithCustomerCommand.Execute(Customer);
                }
            });

            recentItems = new ObservableCollection<Item>();
            RecentItemsCollection = new CollectionViewSource { Source = recentItems };
        }

        public CustomerViewModel(MainViewModel mainViewModel)
        {
            if (mainViewModel != null)
            {
                MainViewModel = mainViewModel;
            }
            
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
                        UpdateFrequentlyBoughtItems();
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
            
            AddCustomerCommand = new RelayCommand(o =>
            {
                var screen = new CreateCustomer();
                screen.Show();
            });

            PasswordStateCommand = new RelayCommand(o =>
            {
                IsShowing = !IsShowing;
            });

            CreateOrderCommand = new RelayCommand(s =>
            {
                if (CustomerFound)
                {
                    MainViewModel.CreateOrderViewWithCustomerCommand.Execute(Customer);
                }
            });

            recentItems = new ObservableCollection<Item>();
            RecentItemsCollection = new CollectionViewSource { Source = recentItems };
        }
        
        public async void UpdateFrequentlyBoughtItems()
        {
            var items = new ObservableCollection<Item>(await DbQueries.CustomerQueries.GetFrequentlyBoughtItemsByCustomerIdAsync(Customer.Id));
            recentItems.Clear();
            foreach(var item in items)
            {
                recentItems.Add(item);
            }
        }
    }
}
