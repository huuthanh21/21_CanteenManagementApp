using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CustomerViewModel : ObservableObject
    {
        public bool CustomerFound { get; set; } = true; // set true for testing
        public Customer Customer { get; set; }
        public ICommand FindCustomerCommand { get; set; }
        public ICommand AddCustomerCommand { get; set; }

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
            // Put the below lines of code outside for testing purposes
            setRecentItems();
            RecentItemsCollection = new CollectionViewSource { Source = recentItems };
            setReceipts();
            ReceiptsCollection = new CollectionViewSource { Source = receipts };


        }
        private void setRecentItems()
        {
            recentItems = new ObservableCollection<Item>
            {
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
            };
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
