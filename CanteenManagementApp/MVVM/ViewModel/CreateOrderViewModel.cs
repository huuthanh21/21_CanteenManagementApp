using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using CanteenManagementApp.Pages;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CreateOrderViewModel : ObservableObject
    {
        public RelayCommand NavigateMainPageCommand { get; set; }
        public RelayCommand NavigatePaymentPageCommand { get; set; }
        public RelayCommand NavigateReceiptPageCommand { get; set; }

        private object _currentPage;

        public object CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }

        public Customer Customer { get; set; } = null;

        private bool _hasCustomer;
        public bool HasCustomer { get => Customer != null; set => _hasCustomer = value; }

        public CreateOrderMainPage CreateOrderMainPage { get; set; }
        public CreateOrderPaymentPage CreateOrderPaymentPage { get; set; }
        public CreateOrderReceiptPage CreateOrderReceiptPage { get; set; }

        //----------------
        public ObservableCollection<Item> _allItems;
        public ObservableCollection<Item> _foodItems;
        public ObservableCollection<Item> _inventoryItems;

        public ObservableCollection<ItemOrder> _ListItemOrder;
        public ObservableCollection<ItemOrder> _TotalItemOrder;
        //private CollectionViewSource StorageItemsCollection;
        private CollectionViewSource InventoryItemsCollection;
        private CollectionViewSource FoodItemsCollection;
        private CollectionViewSource TotalOrderItemsCollection;
        //public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        //private string imageFileName = "";

        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public ICollectionView InventorySourceCollection => InventoryItemsCollection.View;
        public ICollectionView TotalOrderSourceCollection => TotalOrderItemsCollection.View;
        public ICommand IncreaseAmountOrderCommand;
        //----------------
        public CreateOrderViewModel()
        {
            CreateOrderMainPage = new CreateOrderMainPage(this);
            CreateOrderPaymentPage = new CreateOrderPaymentPage(this);
            CreateOrderReceiptPage = new CreateOrderReceiptPage(this);

            CurrentPage = CreateOrderMainPage;

            NavigateMainPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderMainPage;
            });

            NavigatePaymentPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderPaymentPage;
            });

            NavigateReceiptPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderReceiptPage;
            });

            //--------------
            _allItems = new ObservableCollection<Item>
            {

                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 1, Name = "Cocacola", Amount = 10, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 100, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 520, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 420, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 440, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 521, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 1000, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 104, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 1120, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 140, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 150, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 250, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 110, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 1240, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 160, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },

            };

            _ListItemOrder = new ObservableCollection<ItemOrder>
            {
                 new ItemOrder() {  _item = (Item) _allItems[0].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[1].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[2].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[3].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[4].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[5].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[6].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[7].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[8].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[9].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[10].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[11].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[12].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[13].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[14].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[15].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[16].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[17].Clone() }
            };

            _foodItems = new ObservableCollection<Item> { };
            _inventoryItems = new ObservableCollection<Item> { };
            _TotalItemOrder = new ObservableCollection<ItemOrder> { };

            foreach (Item item in _allItems)
            {
                if (item.Type == 1)
                {
                    _inventoryItems.Add(item);
                }
                else
                {
                    _foodItems.Add(item);
                }
            }

            //_ListItemOrder.f
            foreach(ItemOrder itemOrder in _ListItemOrder)
            {
                if (itemOrder._amount > 0)
                {
                    _TotalItemOrder.Add(itemOrder);
                }
            }

            FoodItemsCollection = new CollectionViewSource { Source = _ListItemOrder };
            TotalOrderItemsCollection = new CollectionViewSource { Source = _TotalItemOrder };
            InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };
            IncreaseAmountOrderCommand = new RelayCommand<CreateOrderMainPage>((parameter) => true, (parameter) => IncreaseAmountOrder(parameter));
        }

        private void IncreaseAmountOrder(CreateOrderMainPage parameter)
        {
            int indexSelected = parameter.foodListView.SelectedIndex;
            _ListItemOrder[indexSelected]._amount++;
        }

        public CreateOrderViewModel(Customer customer = null)
        {
            CreateOrderMainPage = new CreateOrderMainPage(this);
            CreateOrderPaymentPage = new CreateOrderPaymentPage(this);
            CreateOrderReceiptPage = new CreateOrderReceiptPage(this);

            CurrentPage = CreateOrderMainPage;
            Customer = customer;

            NavigateMainPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderMainPage;
            });

            NavigatePaymentPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderPaymentPage;
            });

            NavigateReceiptPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderReceiptPage;
            });

            //------------
            _allItems = new ObservableCollection<Item>
            {

                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 1, Name = "Cocacola", Amount = 10, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 100, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 520, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 420, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 440, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 521, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 1000, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 104, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 1120, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 140, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 150, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 250, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 110, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 1240, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 160, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },

            };

            _ListItemOrder = new ObservableCollection<ItemOrder>
            {
                new ItemOrder() {  _item = (Item) _allItems[0].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[1].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[2].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[3].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[4].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[5].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[6].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[7].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[8].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[9].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[10].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[11].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[12].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[13].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[14].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[15].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[16].Clone() },
                new ItemOrder() {  _item = (Item) _allItems[17].Clone() }
            };

            _foodItems = new ObservableCollection<Item> { };
            _inventoryItems = new ObservableCollection<Item> { };
            foreach (Item item in _allItems)
            {
                if (item.Type == 1)
                {
                    _inventoryItems.Add(item);
                }
                else
                {
                    _foodItems.Add(item);
                }
            }

            FoodItemsCollection = new CollectionViewSource { Source = _ListItemOrder };
            InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };
            IncreaseAmountOrderCommand = new RelayCommand<CreateOrderMainPage>((parameter) => true, (parameter) => IncreaseAmountOrder(parameter));
        }

        public void UpdateTotalOrder()
        {
            foreach (ItemOrder itemOrder in _ListItemOrder)
            {
                if (!_TotalItemOrder.Contains(itemOrder))
                {
                    if (itemOrder._amount > 0)
                    {
                        _TotalItemOrder.Add(itemOrder);
                    }
                }
                else
                {
                    if(itemOrder._amount <= 0)
                    {
                        _TotalItemOrder.Remove(itemOrder);
                    }
                }
               
            }
        }
    }
}
