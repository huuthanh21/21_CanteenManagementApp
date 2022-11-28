using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        public ObservableCollection<Item> _allItems;
        public ObservableCollection<Item> _foodItems;
        public ObservableCollection<Item> _inventoryItems;
        //private CollectionViewSource StorageItemsCollection;
        private CollectionViewSource InventoryItemsCollection;
        private CollectionViewSource FoodItemsCollection;
        //public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public ICollectionView InventorySourceCollection => InventoryItemsCollection.View;
        public ICommand EditItemCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditInventoryCommand { get; set; }
        public ICommand DeleteInventoryCommand { get; set; }
        public StorageViewModel()
        {
            _allItems = new ObservableCollection<Item>
            {
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Describe = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 12000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 12000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 12000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 12000, ImagePath = "/Images/b1.jpg" },
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 12000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 12000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Type = 1, Name = "Cocacola", Amount = 10, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 100, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 520, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 420, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 440, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 521, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 1000, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 104, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 1120, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },
                new Item() { Type = 1, Name = "Snack", Amount = 140, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/snack.png" },
                new Item() { Type = 1, Name = "7-Up", Amount = 150, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/7up.jpg" },
                new Item() { Type = 1, Name = "Bò húc", Amount = 250, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/bohuc.png" },
                new Item() { Type = 1, Name = "Cocacola", Amount = 110, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/coca.jpg" },
                new Item() { Type = 1, Name = "Pepsi", Amount = 1240, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/pepsi.png" },
                new Item() { Type = 1, Name = "Sprite", Amount = 160, Describe = "Ngon", Id = 13, Price = 10000,  ImagePath = "/Images/sprite.jpg" },

            };

            _foodItems = new ObservableCollection<Item> { };
            _inventoryItems = new ObservableCollection<Item> { };
            foreach (Item item in _allItems)
            {
                if(item.Type == 1)
                {
                    _inventoryItems.Add(item);
                }
                else
                {
                    _foodItems.Add(item);
                }
            }

            FoodItemsCollection = new CollectionViewSource {  Source = _foodItems };
            InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };
            // ViewBillCommand = new RelayCommand<PayWindow>((parameter) => true, (parameter) => ViewBill(parameter));
            //EditItemCommand = new RelayCommand(StorageView =>  ShowEditItem(StorageView) );
            EditItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditItem(parameter));
            DeleteCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => deleteItem(parameter));
            EditInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditInventoryItem(parameter));
            DeleteInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteInventoryItem(parameter));
        }

        private void DeleteInventoryItem(StorageView parameter)
        {
            int i = parameter.inventoryListView.SelectedIndex;
            _inventoryItems.RemoveAt(i);
        }

        private void EditInventoryItem(StorageView parameter)
        {
            var itemSelected = parameter.inventoryListView.SelectedItem as Item;
            var screen = new EditItem(itemSelected);
            //screen.Show();
            if (screen.ShowDialog() == true)
            {
                _inventoryItems[parameter.inventoryListView.SelectedIndex] = screen.EditedItem;
            }
            else
            {


            }
            screen.Close();
        }

        private void EditItem(StorageView parameter)
        {
            var itemSelected = parameter.foodListView.SelectedItem as Item;
            var screen = new EditItem(itemSelected);
            //screen.Show();
            if (screen.ShowDialog() == true)
            {
               _foodItems[parameter.foodListView.SelectedIndex] = screen.EditedItem;
            }
            else
            {
                

            }
            screen.Close();
        }

        private void deleteItem(StorageView parameter)
        {
            int i = parameter.foodListView.SelectedIndex;
            _foodItems.RemoveAt(i);
        }

       

       
    }
}
