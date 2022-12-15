using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        private readonly ObservableCollection<Item> _foodItems;
        private readonly ObservableCollection<Item> _inventoryItems;

        //private CollectionViewSource StorageItemsCollection;
        private readonly CollectionViewSource InventoryItemsCollection;

        private readonly CollectionViewSource FoodItemsCollection;

        //public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        public static string _imageFileName = "";

        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public ICollectionView InventorySourceCollection => InventoryItemsCollection.View;
        public ICommand EditFoodItemCommand { get; set; } // chỉnh sửa món ăn
        public ICommand DeleteCommand { get; set; } // xóa món ăn
        public ICommand EditInventoryCommand { get; set; } // chỉnh sửa hàng tồn
        public ICommand DeleteInventoryCommand { get; set; } // xóa hàng tồn
        public ICommand AddFoodItemCommand { get; set; } // thêm món ăn hàng ngày
        public ICommand AddItemInventoryCommand { get; set; } // thêm hàng tồn
        public ICommand OKAddFoodItemCommand { get; set; } // Thêm món ăn hàng ngày oke
        public ICommand OKAddInventoryItemCommand { get; set; } // Thêm hàng tồn oke
        public ICommand SaveEditFoodItemCommand { get; set; } // Lưu món ăn hàng ngày
        public ICommand SaveEditInventoryItemCommand { get; set; } // Lưu hàng tồn

        // Thêm hàng:
        public ICommand SelectImageFoodItemCommand { get; set; } //chọn ảnh bên Food Item

        public ICommand SelectImageInventoryItemCommand { get; set; } // chọn ảnh bên Inventory Item

        //public ICommand ButtonAddCommand { get; set; }
        public StorageViewModel()
        {
            _foodItems = new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(0));
            _inventoryItems = new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(1));

            FoodItemsCollection = new CollectionViewSource { Source = _foodItems };
            InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };
            EditFoodItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditFoodItem(parameter));
            DeleteCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteFoodItem(parameter));
            EditInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditInventoryItem(parameter));
            DeleteInventoryCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteInventoryItem(parameter));
            AddFoodItemCommand = new RelayCommand<StorageView>((parameter) => true, async (parameter) => await AddFoodItem(parameter));
            AddItemInventoryCommand = new RelayCommand<StorageView>((parameter) => true, async (parameter) => await AddInventoryItem(parameter));
            SelectImageFoodItemCommand = new RelayCommand<AddFoodItem>((parameter) => true, (parameter) => ChooseImageFoodItem(parameter));
            SelectImageInventoryItemCommand = new RelayCommand<AddInventoryItem>((parameter) => true, (parameter) => ChooseImageInventoryItem(parameter));
            //ButtonAddCommand = new RelayCommand<AddItem>((parameter) => true, (parameter) => ButtonAddClick(parameter));
            OKAddFoodItemCommand = new RelayCommand<AddFoodItem>((parameter) => true, (parameter) => OKAddFoodItem(parameter));
            OKAddInventoryItemCommand = new RelayCommand<AddInventoryItem>((parameter) => true, (parameter) => OKAddInventoryItem(parameter));
            SaveEditFoodItemCommand = new RelayCommand<EditFoodItem>((parameter) => true, (parameter) => SaveEditFoodItem(parameter));
            SaveEditInventoryItemCommand = new RelayCommand<EditInventoryItem>((parameter) => true, (parameter) => SaveEditInventoryItem(parameter));
        }

        public void OKAddInventoryItem(AddInventoryItem parameter)
        {
            parameter.DialogResult = true;
        }

        public void OKAddFoodItem(AddFoodItem parameter)
        {
            parameter.DialogResult = true;
        }

        public async Task AddInventoryItem(StorageView parameter)
        {
            var screen = new AddInventoryItem();
            if (screen.ShowDialog() == true)
            {
                var name_template = screen.nameTextBox.Template;
                var control_name = (TextBox)name_template.FindName("TextboxInput", screen.nameTextBox);
                string name = control_name.Text;
                var description_template = screen.describeTextBox.Template;
                var control_description = (TextBox)description_template.FindName("TextboxInput", screen.describeTextBox);
                string description = control_description.Text;
                var price_template = screen.priceTextBox.Template;
                var control_price = (TextBox)price_template.FindName("TextboxInput", screen.priceTextBox);
                string price = control_price.Text;
                var amount_template = screen.amountTextBox.Template;
                var control_amount = (TextBox)amount_template.FindName("TextboxInput", screen.amountTextBox);
                string amount = control_amount.Text;
                Item NewItem = new()
                {
                    Name = name,
                    Price = float.Parse(price),
                    Description = description,
                    Amount = int.Parse(amount),
                    Type = 1
                };

                await DbQueries.ItemQueries.InsertItemAsync(NewItem);
                _inventoryItems.Add(NewItem);
                /*CopyFileToAppFolder(NewItem.Id.ToString(), _imageFileName);*/
            }
            screen.Close();
        }

        public async Task AddFoodItem(StorageView parameter)
        {
            var screen = new AddFoodItem();
            screen.ShowDialog();
            if (screen.DialogResult == true)
            {
                // Get item's name
                var name_template = screen.nameTextBox.Template;
                var control_name = (TextBox)name_template.FindName("TextboxInput", screen.nameTextBox);
                string name = control_name.Text;

                // Get item's description
                var description_template = screen.describeTextBox.Template;
                var control_description = (TextBox)description_template.FindName("TextboxInput", screen.describeTextBox);
                string description = control_description.Text;

                // Get item's price
                var price_template = screen.priceTextBox.Template;
                var control_price = (TextBox)price_template.FindName("TextboxInput", screen.priceTextBox);
                string price = control_price.Text;

                Item newItem = new()
                {
                    Name = name,
                    Price = float.Parse(price),
                    Description = description,
                    Amount = 0,
                    Type = 0
                };

                _foodItems.Add(newItem);
                await DbQueries.ItemQueries.InsertItemAsync(newItem);
                CopyFileToAppFolder(newItem.Id.ToString(), _imageFileName);
            }
            screen.Close();
        }

        public static void CopyFileToAppFolder(string id, string sourceFileName)
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = "21CanteenManager";

            var appPath = Path.Combine(appdataPath, appFolder);
            StringBuilder stringBuilder = new();
            stringBuilder.Append(id);
            stringBuilder.Append(".jpg");

            var filePath = Path.Combine(appPath, stringBuilder.ToString());

            File.Copy(sourceFileName, filePath);
        }

        public static void ChooseImageInventoryItem(AddInventoryItem parameter)
        {
            OpenFileDialog op = new()
            {
                Title = "Chọn ảnh",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new();
                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(_imageFileName);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                parameter.grdSelectImg.Background = imageBrush;
                parameter.imageEmpty.Visibility = Visibility.Hidden;
            }
        }

        public static void ChooseImageFoodItem(AddFoodItem parameter)
        {
            OpenFileDialog op = new()
            {
                Title = "Chọn ảnh",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new();
                BitmapImage bitmap = new();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(_imageFileName);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                parameter.grdSelectImg.Background = imageBrush;
                parameter.imageEmpty.Visibility = Visibility.Hidden;
            }
        }

        //private void SelectImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog op = new OpenFileDialog();
        //    op.Title = "Chọn ảnh";
        //    op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
        //    if (op.ShowDialog() == true)
        //    {
        //        _imageFileName = op.FileName;
        //        ImageBrush imageBrush = new ImageBrush();
        //        BitmapImage bitmap = new BitmapImage();
        //        bitmap.BeginInit();
        //        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmap.UriSource = new Uri(_imageFileName);
        //        bitmap.EndInit();
        //        imageBrush.ImageSource = bitmap;
        //        parameter.grdSelectImg.Background = imageBrush;
        //        parameter.imageEmpty.Visibility = Visibility.Hidden;
        //    }
        //}

        private void DeleteInventoryItem(StorageView parameter)
        {
            int i = parameter.inventoryListView.SelectedIndex;
            _inventoryItems.RemoveAt(i);
        }

        private void SaveEditInventoryItem(EditInventoryItem parameter)
        {
            parameter.DialogResult = true;
        }

        private void EditInventoryItem(StorageView parameter)
        {
            var itemSelected = parameter.inventoryListView.SelectedItem as Item;
            var screen = new EditInventoryItem(itemSelected);
            screen.IdTextBox.Text = itemSelected.Id.ToString();
            screen.nameTextBox.Text = itemSelected.Name;
            screen.priceTextBox.Text = itemSelected.Price.ToString();
            screen.describeTextBox.Text = itemSelected.Description;
            screen.amountTextBox.Text = itemSelected.Amount.ToString();
            if (screen.ShowDialog() == true)
            {
                Item NewItem = new()
                {
                    Id = itemSelected.Id,
                    Name = screen.nameTextBox.Text,
                    Price = float.Parse(screen.priceTextBox.Text),
                    Description = screen.describeTextBox.Text,
                    Amount = int.Parse(screen.amountTextBox.Text),
                    Type = 1
                };
                DbQueries.ItemQueries.UpdateItem(int.Parse(screen.IdTextBox.Text), screen.nameTextBox.Text, float.Parse(screen.priceTextBox.Text), screen.describeTextBox.Text, int.Parse(screen.amountTextBox.Text));
                _inventoryItems[parameter.inventoryListView.SelectedIndex] = NewItem;
            }
            screen.Close();
        }

        private void SaveEditFoodItem(EditFoodItem parameter)
        {
            parameter.DialogResult = true;
        }

        private void EditFoodItem(StorageView parameter)
        {
            var itemSelected = parameter.foodListView.SelectedItem as Item;
            var screen = new EditFoodItem(itemSelected);
            screen.IdTextBox.Text = itemSelected.Id.ToString();
            screen.nameTextBox.Text = itemSelected.Name;
            screen.priceTextBox.Text = itemSelected.Price.ToString();
            screen.describeTextBox.Text = itemSelected.Description;
            if (screen.ShowDialog() == true)
            {
                Item NewItem = new()
                {
                    Id = int.Parse(screen.IdTextBox.Text),
                    Name = screen.nameTextBox.Text,
                    Price = float.Parse(screen.priceTextBox.Text),
                    Description = screen.describeTextBox.Text,
                    Amount = 0,
                    Type = 0
                };
                DbQueries.ItemQueries.UpdateItem(int.Parse(screen.IdTextBox.Text), screen.nameTextBox.Text, float.Parse(screen.priceTextBox.Text), screen.describeTextBox.Text, itemSelected.Amount);
                _foodItems[parameter.foodListView.SelectedIndex] = NewItem;
            }
            screen.Close();
        }

        private void DeleteFoodItem(StorageView parameter)
        {
            int i = parameter.foodListView.SelectedIndex;
            _foodItems.RemoveAt(i);
        }
    }
}