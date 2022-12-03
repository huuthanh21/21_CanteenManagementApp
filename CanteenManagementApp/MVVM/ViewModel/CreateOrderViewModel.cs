using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using CanteenManagementApp.Pages;
using Microsoft.Identity.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
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

        public static Customer Customer { get; set; } = null;
        public static bool HasCustomer { get => Customer != null; set { } }

        public CreateOrderMainPage CreateOrderMainPage { get; set; }
        public CreateOrderPaymentPage CreateOrderPaymentPage { get; set; }
        public CreateOrderReceiptPage CreateOrderReceiptPage { get; set; }

        //----------------
        public ObservableCollection<Item> _foodItems;
        public ObservableCollection<Item> _inventoryItems;

        public ObservableCollection<ItemOrder> _ListFoodItemOrder;
        public ObservableCollection<ItemOrder> _ListInventoryItemOrder;
        // total items selected by customer
        public ObservableCollection<ItemOrder> _TotalItemOrder;

        private CollectionViewSource InventoryItemsCollection;
        private CollectionViewSource FoodItemsCollection;
        private CollectionViewSource TotalOrderItemsCollection;

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
            // hard data assignment
            _foodItems = new ObservableCollection<Item>
            {

                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
               
              
            };
            _inventoryItems = new ObservableCollection<Item>
            {
                new Item() { Type = 1, Name = "Cocacola", Amount = 10, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 100, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 520, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 420, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 440, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 521, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 1000, Description = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
              
            };


            // set up data to display
            _ListFoodItemOrder = new ObservableCollection<ItemOrder>{ };
            _ListInventoryItemOrder = new ObservableCollection<ItemOrder> { };

            foreach (Item item in _foodItems)
            {
                _ListFoodItemOrder.Add(new ItemOrder() { _item = (Item) item.Clone() }) ;
            }
            foreach (Item item in _inventoryItems)
            {
                _ListInventoryItemOrder.Add(new ItemOrder() { _item = (Item)item.Clone() });
            }

            _TotalItemOrder = new ObservableCollection<ItemOrder> { };
            

            FoodItemsCollection = new CollectionViewSource { Source = _ListFoodItemOrder };
            TotalOrderItemsCollection = new CollectionViewSource { Source = _TotalItemOrder };
            InventoryItemsCollection = new CollectionViewSource { Source = _ListInventoryItemOrder };

            IncreaseAmountOrderCommand = new RelayCommand<CreateOrderMainPage>((parameter) => true, (parameter) => IncreaseAmountOrder(parameter));
        }

        private void IncreaseAmountOrder(CreateOrderMainPage parameter)
        {
            var screen = new AddItem(0);
            //int indexSelected = parameter.foodListView.SelectedIndex;
            //_ListFoodItemOrder[indexSelected]._amount++;
            if (screen.ShowDialog() == true)
            {
                if (screen.NewItem.Id != 0 || screen.NewItem.Name != "" || screen.NewItem.Price != 0
                    || screen.NewItem.Description != "" || screen.NewItem.ImagePath != "")
                {
                    _foodItems.Add(screen.NewItem.Clone() as Item);
                }

            }
            else
            {


            }
            screen.Close();

        }

        public void UpdateTotalOrder()
        {
            foreach (ItemOrder itemOrder in _ListFoodItemOrder)
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
            foreach (ItemOrder itemOrder in _ListInventoryItemOrder)
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
                    if (itemOrder._amount <= 0)
                    {
                        _TotalItemOrder.Remove(itemOrder);
                    }
                }

            }

        }
    }
}
