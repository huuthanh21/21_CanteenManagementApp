using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;

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

        public ObservableCollection<Receipt> receipts;

        private readonly CollectionViewSource RecentItemsCollection;
        private readonly CollectionViewSource ReceiptsCollection;

        public ICollectionView RecentItemsSourceCollection => RecentItemsCollection.View;
        public ICollectionView ReceiptsSourceCollection => ReceiptsCollection.View;

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
            setReceipts();
            ReceiptsCollection = new CollectionViewSource { Source = receipts };
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
            recentItems = new ObservableCollection<Item>();
            RecentItemsCollection = new CollectionViewSource { Source = recentItems };
            setReceipts();
            ReceiptsCollection = new CollectionViewSource { Source = receipts };
            
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

        private void setReceipts()
        {
            receipts = new ObservableCollection<Receipt>
            {
                new Receipt() {
                    Receipt_Items= new List<Receipt_Item>
                    {
                        new Receipt_Item() {
                            ItemId = 100000,
                            Amount = 1 },
                        new Receipt_Item() {
                            ItemId = 100001,
                            Amount = 2 }
                    },
                    Id = 10,
                    Total = 35000,
                    DateTime = new System.DateTime(2022, 11, 11),
                 },
                new Receipt() {
                    Receipt_Items= new List<Receipt_Item>
                    {
                        new Receipt_Item() {
                            ItemId = 110000,
                            Amount = 1 },
                    },
                    Id = 21,
                    Total = 30000,
                    DateTime = new System.DateTime(2022, 11, 9),
                 },
                new Receipt() {
                    Receipt_Items= new List<Receipt_Item>
                    {
                        new Receipt_Item() {
                            ItemId = 110002,
                            Amount = 1 },
                        new Receipt_Item() {
                            ItemId = 110003,
                            Amount = 2 },
                    },
                    Id = 21,
                    Total = 32000,
                    DateTime = new System.DateTime(2022, 11, 11)
                 },

            };
        }
    }
}
