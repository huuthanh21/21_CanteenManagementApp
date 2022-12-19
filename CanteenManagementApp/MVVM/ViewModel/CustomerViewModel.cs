using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
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
        public RelayCommand TopUpCommand { get; set; }
        public RelayCommand ViewReceiptDetailsCommand { get; set; }

        private readonly ObservableCollection<Item> _recentItems;

        private readonly ObservableCollection<Receipt> _receipts;

        private readonly CollectionViewSource RecentItemsCollection;
        private readonly CollectionViewSource ReceiptsCollection;

        public ICollectionView RecentItemsSourceCollection => RecentItemsCollection.View;
        public ICollectionView ReceiptsSourceCollection => ReceiptsCollection.View;

        //
        // Default constructor just for setting view's Datacontext purposes
        //
        public CustomerViewModel()
        {
            // Initialize collections so that view can be created without throwing null exception
            _receipts = new ObservableCollection<Receipt>();
            ReceiptsCollection = new CollectionViewSource { Source = _receipts };

            _recentItems = new ObservableCollection<Item>();
            RecentItemsCollection = new CollectionViewSource { Source = _recentItems };
        }

        public CustomerViewModel(MainViewModel mainViewModel)
        {
            if (mainViewModel != null)
            {
                MainViewModel = mainViewModel;
            }

            _receipts = new ObservableCollection<Receipt>();
            ReceiptsCollection = new CollectionViewSource { Source = _receipts };

            _recentItems = new ObservableCollection<Item>();
            RecentItemsCollection = new CollectionViewSource { Source = _recentItems };

            FindCustomerCommand = new RelayCommand<TextBox>(tb => true, async tb =>
            {
                var template = tb.Template;
                var textbox = (TextBox)template.FindName("TextboxInput", tb);
                if (!string.IsNullOrEmpty(textbox.Text))
                {
                    Customer = DbQueries.CustomerQueries.GetCustomerById(textbox.Text);

                    if (Customer != null)
                    {
                        CustomerFound = true;
                        UpdateCustomerReceipts();
                        await UpdateFrequentlyBoughtItems();
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

            TopUpCommand = new RelayCommand(o =>
            {
                if (CustomerFound)
                {
                    var screen = new TopUpDialog(this);
                    screen.ShowDialog();
                    int amount;
                    if (screen.DialogResult == true)
                    {
                        amount = screen.TopUpAmount;
                        screen.Close();
                        MainViewModel.CreateOrderViewWithTopUp.Execute(new Tuple<Customer, int>(Customer, amount));
                    }
                }
            });

            ViewReceiptDetailsCommand = new RelayCommand(receciptId =>
            {
                var screen = new ReceiptView((int)receciptId);
                screen.ShowDialog();

                if (screen.DialogResult == true)
                {
                    screen.Close();
                }
            });
        }

        private async Task UpdateFrequentlyBoughtItems()
        {
            var items = new ObservableCollection<Item>(await DbQueries.CustomerQueries.GetFrequentlyBoughtItemsByCustomerIdAsync(Customer.Id));
            _recentItems.Clear();
            foreach (var item in items)
            {
                _recentItems.Add(item);
            }
        }

        private void UpdateCustomerReceipts()
        {
            var receipts = new ObservableCollection<Receipt>(DbQueries.ReceiptQueries.GetReceiptsByCustomerId(Customer.Id));
            _receipts.Clear();
            foreach (var receipt in receipts)
            {
                _receipts.Add(receipt);
            }
        }
    }
}