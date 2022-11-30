using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;


namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        public ObservableCollection<Item> _allItems;
        public ObservableCollection<Item> _foodItems;
        public ObservableCollection<Item> _inventoryItems;
        //private CollectionViewSource StorageItemsCollection;
        private readonly CollectionViewSource InventoryItemsCollection;
        private readonly CollectionViewSource FoodItemsCollection;
        //public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        private string imageFileName = "";

        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public ICollectionView InventorySourceCollection => InventoryItemsCollection.View;
        public ICommand EditItemCommand { get; set; } // chỉnh sửa món ăn
        public ICommand DeleteCommand { get; set; } // xóa món ăn
        public ICommand EditInventoryCommand { get; set; } // chỉnh sửa hàng tồn
        public ICommand DeleteInventoryCommand { get; set; } // xóa hàng tồn
        public ICommand AddItemCommand { get; set; } // thêm hàng
        public ICommand AddItemInventoryCommand { get; set; } // thêm hàng

        // Thêm hàng:
        //public ICommand SelectImageCommand { get; set; } //chọn ảnh
        //public ICommand ButtonAddCommand { get; set; } 
        public StorageViewModel()
        {
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

            EditItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditItem(parameter));
            DeleteCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteItem(parameter));
            EditInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditInventoryItem(parameter));
            DeleteInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteInventoryItem(parameter));
            AddItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => addItem(parameter));
            AddItemInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => addItemInventory(parameter));
            //SelectImageCommand = new RelayCommand<AddItem>((parameter) => true, (parameter) => ChooseImage(parameter));
            //ButtonAddCommand = new RelayCommand<AddItem>((parameter) => true, (parameter) => ButtonAddClick(parameter));
        }

        private void addItemInventory(StorageView parameter)
        {
            var screen = new AddItem(1);
            if (screen.ShowDialog() == true)
            {
                if (screen.NewItem.Id != 0 || screen.NewItem.Name != "" || screen.NewItem.Price != 0
                    || screen.NewItem.Description != "" || screen.NewItem.ImagePath != "")
                {
                    _inventoryItems.Add(screen.NewItem.Clone() as Item);
                }

            }
            else
            {


            }
            screen.Close();
        }

        //private void ButtonAddClick(AddItem parameter)
        //{
        //    parameter.DialogResult = true;
        //    string id = parameter.IdTextBox.Text;
        //    string name = parameter.IdTextBox.Text;
        //    string description = parameter.descriptionTextBox.Text;
        //    string price = parameter.priceTextBox.Text;
        //    Item newItem = new()
        //    {
        //        Id = int.Parse(id),
        //        Name = name,
        //        Price = double.Parse(price),
        //        Description = description,
        //        Amount = 0,
        //        ImagePath = imageFileName,
        //        Type = 0

        //    };
        //    _foodItems.Add(newItem);
        //}

        //private void ChooseImage(AddItem parameter)
        //{
        //    OpenFileDialog op = new OpenFileDialog();
        //    op.Title = "Chọn ảnh";
        //    op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
        //    if (op.ShowDialog() == true)
        //    {
        //        imageFileName = op.FileName;
        //        ImageBrush imageBrush = new ImageBrush();
        //        BitmapImage bitmap = new BitmapImage();
        //        bitmap.BeginInit();
        //        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmap.UriSource = new Uri(imageFileName);
        //        bitmap.EndInit();
        //        imageBrush.ImageSource = bitmap;
        //        parameter.grdSelectImg.Background = imageBrush;
        //    }
        //}

        private void addItem(StorageView parameter)
        {
            var screen = new AddItem(0);
            //screen.Show();
            //if (screen.ShowDialog() == true)
            //{
            //    if (screen.NewBook.Name != "" || screen.NewBook.PublishedYear != "" || screen.NewBook.Author != "")
            //    {
            //        _allItems.Add(screen.NewBook.Clone() as Book);
            //    }
            //}
            if (screen.ShowDialog() == true)
            {
                if(screen.NewItem.Id != 0 || screen.NewItem.Name != "" || screen.NewItem.Price != 0 
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

        private void DeleteItem(StorageView parameter)
        {
            int i = parameter.foodListView.SelectedIndex;
            _foodItems.RemoveAt(i);
        }

       

       
    }
}
