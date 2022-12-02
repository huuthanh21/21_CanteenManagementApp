using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;


namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        /*public ObservableCollection<Item> _allItems;*/
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
        public ICommand AddFoodItemCommand { get; set; } // thêm hàng
        public ICommand AddInventoryItemCommand { get; set; }

        // Thêm hàng:
        //public ICommand SelectImageCommand { get; set; } //chọn ảnh
        //public ICommand ButtonAddCommand { get; set; } 
        public StorageViewModel()
        {
            /*var items = DbQueries.ItemQueries.GetAllItem();
            foreach (var item in items)
            {
                _allItems.Add( item );
            } */   
            _foodItems = new ObservableCollection<Item> (DbQueries.ItemQueries.GetItemsByType(0));
            _inventoryItems = new ObservableCollection<Item> (DbQueries.ItemQueries.GetItemsByType(1));

           /* foreach (Item item in _allItems)
            {
                if(item.Type == 1)
                {
                    _inventoryItems.Add(item);
                }
                else
                {
                    _foodItems.Add(item);
                }
            }*/

            FoodItemsCollection = new CollectionViewSource {  Source = _foodItems };
            InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };

            EditItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditItem(parameter));
            DeleteCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteItem(parameter));
            EditInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditInventoryItem(parameter));
            DeleteInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteInventoryItem(parameter));
            AddFoodItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => addFoodItem(parameter));
            AddInventoryItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => addInventoryItem(parameter));

            //SelectImageCommand = new RelayCommand<AddItem>((parameter) => true, (parameter) => ChooseImage(parameter));
            //ButtonAddCommand = new RelayCommand<AddItem>((parameter) => true, (parameter) => ButtonAddClick(parameter));
        }

        //private void ButtonAddClick(AddItem parameter)
        //{
        //    parameter.DialogResult = true;
        //    string id = parameter.IdTextBox.Text;
        //    string name = parameter.IdTextBox.Text;
        //    string describe = parameter.describeTextBox.Text;
        //    string price = parameter.priceTextBox.Text;
        //    Item newItem = new()
        //    {
        //        Id = int.Parse(id),
        //        Name = name,
        //        Price = double.Parse(price),
        //        Description = describe,
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

        private void addFoodItem(StorageView parameter)
        {
            var screen = new AddFoodItem();
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

        private void addInventoryItem(StorageView parameter)
        {
            var screen = new AddInventoryItem();
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
