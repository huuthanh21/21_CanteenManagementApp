using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;
using System.Diagnostics;

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

        private readonly ObservableCollection<Item> _recentItems;

        private ObservableCollection<Receipt> _receipts;

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
            SetDemoReceipts();
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

            SetDemoReceipts();
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

        private void SetDemoReceipts()
        {
            _receipts = new ObservableCollection<Receipt>
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
            };
        }
    }
}