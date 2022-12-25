using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using PropertyChanged;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        /*public ObservableCollection<Item> _allItems;*/
        public ObservableCollection<Item> _foodItems;
        public ObservableCollection<Item> _tempfoodItems;
        public ObservableCollection<Item> _inventoryItems;
        //private CollectionViewSource StorageItemsCollection;
        private CollectionViewSource InventoryItemsCollection;
        private CollectionViewSource FoodItemsCollection;
        //public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        public static string _imageFileName = "";

        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public ICollectionView InventorySourceCollection => InventoryItemsCollection.View;
        public ICommand EditFoodItemCommand { get; set; } // chỉnh sửa món ăn
        public ICommand DeleteFoodItemCommand { get; set; } // xóa món ăn
        public ICommand EditInventoryItemCommand { get; set; } // chỉnh sửa hàng tồn
        public ICommand DeleteInventoryItemCommand { get; set; } // xóa hàng tồn
        public ICommand AddFoodItemCommand { get; set; } // thêm món ăn hàng ngày
        public ICommand AddInventoryItemCommand { get; set; } // thêm hàng tồn
        public ICommand OKAddFoodItemCommand { get; set; } // Thêm món ăn hàng ngày oke
        public ICommand OKAddInventoryItemCommand { get; set; } // Thêm hàng tồn oke
        public ICommand SaveEditFoodItemCommand { get; set; } // Lưu món ăn hàng ngày
        public ICommand SaveEditInventoryItemCommand { get; set; } // Lưu hàng tồn
        // Thêm hàng:
        public ICommand SelectImageFoodItemCommand { get; set; } //chọn ảnh bên Food Item
        public ICommand SelectImageInventoryItemCommand { get; set; } // chọn ảnh bên Inventory Item
        //public ICommand ButtonAddCommand { get; set; } 
        // Sửa ảnh
        public ICommand EditImageFoodItemCommand { get; set; } // Sửa ảnh bên Food Item
        public ICommand EditImageInventoryItemCommand { get; set; } // Sửa ảnh bên Inventory Item

        public StorageViewModel()
        {
            _foodItems = new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(0));
            _inventoryItems = new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(1));

            FoodItemsCollection = new CollectionViewSource { Source = _foodItems };
            InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };
            EditFoodItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditFoodItem(parameter));
            DeleteFoodItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteFoodItem(parameter));
            EditInventoryItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditInventoryItem(parameter));
            DeleteInventoryItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => DeleteInventoryItem(parameter));
            AddFoodItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => AddFoodItem(parameter));
            AddInventoryItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => AddInventoryItem(parameter));
            SelectImageFoodItemCommand = new RelayCommand<AddFoodItem>((parameter) => true, (parameter) => ChooseImageFoodItem(parameter));
            SelectImageInventoryItemCommand = new RelayCommand<AddInventoryItem>((parameter) => true, (parameter) => ChooseImageInventoryItem(parameter));
            //ButtonAddCommand = new RelayCommand<AddItem>((parameter) => true, (parameter) => ButtonAddClick(parameter));
            OKAddFoodItemCommand = new RelayCommand<AddFoodItem>((parameter) => true, (parameter) => OKAddFoodItem(parameter));
            OKAddInventoryItemCommand = new RelayCommand<AddInventoryItem>((parameter) => true, (parameter) => OKAddInventoryItem(parameter));
            SaveEditFoodItemCommand = new RelayCommand<EditFoodItem>((parameter) => true, (parameter) => SaveEditFoodItem(parameter));
            SaveEditInventoryItemCommand = new RelayCommand<EditInventoryItem>((parameter) => true, (parameter) => SaveEditInventoryItem(parameter));
            EditImageFoodItemCommand = new RelayCommand<EditFoodItem>((parameter) => true, (parameter) => EditImageFoodItem(parameter));
            EditImageInventoryItemCommand = new RelayCommand<EditInventoryItem>((parameter) => true, (parameter) => EditImageInventoryItem(parameter));
            FoodSourceCollection.Filter = ItemFilter;
            InventorySourceCollection.Filter = ItemFilter;
            TextSearchBarChanged += RefreshItemViewSource;
        }
        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private static BitmapImage GetBitmap(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appFolder = "21CanteenManager";

                var appPath = Path.Combine(appdataPath, appFolder);

                var defaultImageName = "default.jpg";
                var defaultImagePath = Path.Combine(appPath, defaultImageName);
                filePath = defaultImagePath;
            }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = new Uri(filePath);
            bitmap.EndInit();
            return bitmap;
        }
        private object _textSearchBar;

        private readonly EventHandler TextSearchBarChanged;
        public object TextSearchBar
        {
            get { return _textSearchBar; }
            set
            {
                _textSearchBar = value;
                OnTextSearchBarChanged(EventArgs.Empty);
            }
        }
        public bool ItemFilter(object item)
        {
            if (string.IsNullOrEmpty(TextSearchBar as string))
            {
                return true;
            }

            return (item as Item).Name.Contains(TextSearchBar as string, StringComparison.OrdinalIgnoreCase);
        }

        private void RefreshItemViewSource(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(FoodSourceCollection).Refresh();
            CollectionViewSource.GetDefaultView(InventorySourceCollection).Refresh();
        }

        [SuppressPropertyChangedWarnings]
        public void OnTextSearchBarChanged(EventArgs e)
        {
            TextSearchBarChanged?.Invoke(this, e);
        }

        private void EditImageInventoryItem(EditInventoryItem parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = GetBitmap(_imageFileName);
                imageBrush.ImageSource = bitmap;
                parameter.grdSelectImg.Background = imageBrush;
                parameter.imageEmpty.Visibility = Visibility.Hidden;
            }
        }

        private void EditImageFoodItem(EditFoodItem parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = GetBitmap(_imageFileName);
                imageBrush.ImageSource = bitmap;
                parameter.grdSelectImg.Background = imageBrush;
                parameter.imageEmpty.Visibility = Visibility.Hidden;
            }
        }

        public void OKAddInventoryItem(AddInventoryItem parameter)
        {

            // 1: Lỗi để trống các textBox
            // 2: Lỗi nhập giá cả âm hoặc ký tự lạ
            // 3: Lỗi nhập số lượng âm hoặc ký tự lạ

            int error_type = 0;
            var price_template = parameter.priceTextBox.Template;
            var control_price = (TextBox)price_template.FindName("TextboxInput", parameter.priceTextBox);
            string price = control_price.Text;
            float price_test = 0;

            var name_template = parameter.nameTextBox.Template;
            var control_name = (TextBox)name_template.FindName("TextboxInput", parameter.nameTextBox);
            string name = control_name.Text;

            var description_template = parameter.describeTextBox.Template;
            var control_description = (TextBox)description_template.FindName("TextboxInput", parameter.describeTextBox);
            string description = control_description.Text;

            var amount_template = parameter.amountTextBox.Template;
            var control_amount = (TextBox)amount_template.FindName("TextboxInput", parameter.amountTextBox);
            string amount = control_amount.Text;
            int amount_test = 0;

            //1. 
            if (price == "" || name == "" || description == "" || amount == "")
            {
                error_type = 1;
            }
            //2.
            else if (float.TryParse(price, out price_test) == false || float.Parse(price) < 0)
            {
                error_type = 2;
            }
            else
            {
                bool check = int.TryParse(amount, out amount_test);
                if (check == true)
                {
                    amount_test = int.Parse(amount);
                    if (amount_test < 0)
                    {
                        error_type = 3;
                    }
                }
                else error_type = 3;
            }
            switch (error_type)
            {
                case 1:
                    MessageBox.Show("Lỗi! Còn một số trường chưa được nhập giá trị");
                    break;
                case 2:
                    MessageBox.Show("Lỗi! Giá cả nhập vào không hợp lệ");
                    break;
                case 3:
                    MessageBox.Show("Lỗi! Số lượng nhập vào không hợp lệ");
                    break;
                default:
                    MessageBox.Show("Thêm hàng tồn thành công");
                    parameter.DialogResult = true;
                    break;
            }
        }

        public void OKAddFoodItem(AddFoodItem parameter)
        {
            // 1: Lỗi để trống các textBox
            // 2: Lỗi nhập giá cả âm hoặc ký tự lạ

            int error_type = 0;
            var price_template = parameter.priceTextBox.Template;
            var control_price = (TextBox)price_template.FindName("TextboxInput", parameter.priceTextBox);
            string price = control_price.Text;

            var name_template = parameter.nameTextBox.Template;
            var control_name = (TextBox)name_template.FindName("TextboxInput", parameter.nameTextBox);
            string name = control_name.Text;

            var description_template = parameter.describeTextBox.Template;
            var control_description = (TextBox)description_template.FindName("TextboxInput", parameter.describeTextBox);
            string description = control_description.Text;

            //1. 
            if (price == "" || name == "" || description == "")
            {
                error_type = 1;
            }
            //2.
            else
            {
                float price_test = 0;
                bool check = float.TryParse(price, out price_test);
                if (check == true)
                {
                    price_test = float.Parse(price);
                    if (price_test < 0)
                    {
                        error_type = 2;
                    }
                }
                else error_type = 2;
            }
            switch (error_type)
            {
                case 1:
                    MessageBox.Show("Lỗi! Còn một số trường chưa được nhập giá trị");
                    break;
                case 2:
                    MessageBox.Show("Lỗi! Giá cả nhập vào không hợp lệ");
                    break;
                default:
                    MessageBox.Show("Thêm món ăn hàng ngày thành công");
                    parameter.DialogResult = true;
                    break;
            }
        }

        public async void AddInventoryItem(StorageView parameter)
        {
            _imageFileName = "";
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

                await DbQueries.ItemQueries.InsertItem_Async(NewItem);
                _inventoryItems.Add(NewItem);
                CopyFileToAppFolder(NewItem.Id.ToString(), _imageFileName);
            }
            else
            {


            }
            screen.Close();
        }

        public async void AddFoodItem(StorageView parameter)
        {
            _imageFileName = "";
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

                Item NewItem = new()
                {
                    Name = name,
                    Price = float.Parse(price),
                    Description = description,
                    Amount = 0,
                    Type = 0
                };
                await DbQueries.ItemQueries.InsertItem_Async(NewItem);
                _foodItems.Add(NewItem);
                CopyFileToAppFolder(NewItem.Id.ToString(), _imageFileName);

            }
            else
            {

            }
        }

        public static void CopyFileToAppFolder(string id, string sourceFileName)
        {
            if (!sourceFileName.Equals(""))
            {
                var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appFolder = "21CanteenManager";

                var appPath = Path.Combine(appdataPath, appFolder);
                StringBuilder stringBuilder = new();
                stringBuilder.Append(id);
                stringBuilder.Append(".jpg");

                var filePath = Path.Combine(appPath, stringBuilder.ToString());

                File.Copy(sourceFileName, filePath, true);
            }
        }

        public string GetFilePath(string id)
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = "21CanteenManager";

            var appPath = Path.Combine(appdataPath, appFolder);
            StringBuilder stringBuilder = new();
            stringBuilder.Append(id);
            stringBuilder.Append(".jpg");

            var filePath = Path.Combine(appPath, stringBuilder.ToString());
            return filePath;
        }

        public static void ChooseImageInventoryItem(AddInventoryItem parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = GetBitmap(_imageFileName);
                imageBrush.ImageSource = bitmap;
                parameter.grdSelectImg.Background = imageBrush;
                parameter.imageEmpty.Visibility = Visibility.Hidden;
            }
        }

        public static void ChooseImageFoodItem(AddFoodItem parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = GetBitmap(_imageFileName);
                imageBrush.ImageSource = bitmap;
                parameter.grdSelectImg.Background = imageBrush;
                parameter.imageEmpty.Visibility = Visibility.Hidden;
            }
        }

        private void SaveEditInventoryItem(EditInventoryItem parameter)
        {
            // 1: Lỗi để trống các textBox
            // 2: Lỗi nhập giá cả âm hoặc ký tự lạ
            // 3: Lỗi nhập số lượng âm hoặc ký tự lạ

            int error_type = 0;
            string price = parameter.priceTextBox.Text;
            float price_test = 0;
            string name = parameter.nameTextBox.Text;
            string description = parameter.describeTextBox.Text;
            string amount = parameter.amountTextBox.Text;
            int amount_test = 0;

            //1. 
            if (price == "" || name == "" || description == "" || amount == "")
            {
                error_type = 1;
            }
            //2.
            else if (float.TryParse(price, out price_test) == false || float.Parse(price) < 0)
            {
                error_type = 2;
            }
            else
            {
                bool check = int.TryParse(amount, out amount_test);
                if (check == true)
                {
                    amount_test = int.Parse(amount);
                    if (amount_test < 0)
                    {
                        error_type = 3;
                    }
                }
                else error_type = 3;
            }
            switch (error_type)
            {
                case 1:
                    MessageBox.Show("Lỗi! Còn một số trường chưa được nhập giá trị");
                    break;
                case 2:
                    MessageBox.Show("Lỗi! Giá cả nhập vào không hợp lệ");
                    break;
                case 3:
                    MessageBox.Show("Lỗi! Số lượng nhập vào không hợp lệ");
                    break;
                default:
                    MessageBox.Show("Chỉnh sửa hàng tồn thành công");
                    parameter.DialogResult = true;
                    break;
            }
        }

        private void EditInventoryItem(StorageView parameter)
        {
            _imageFileName = "";
            var itemSelected = parameter.inventoryListView.SelectedItem as Item;
            var screen = new EditInventoryItem(itemSelected);
            screen.IdTextBox.Text = itemSelected.Id.ToString();
            screen.nameTextBox.Text = itemSelected.Name;
            screen.priceTextBox.Text = itemSelected.Price.ToString();
            screen.describeTextBox.Text = itemSelected.Description;
            screen.amountTextBox.Text = itemSelected.Amount.ToString();

            ImageBrush imageBrush = new ImageBrush();
            BitmapImage bitmap = GetBitmap(GetFilePath(itemSelected.Id.ToString()));
            imageBrush.ImageSource = bitmap;
            screen.grdSelectImg.Background = imageBrush;
            screen.imageEmpty.Visibility = Visibility.Hidden;

            // Set temp image
            //parameter.foodListView.ItemsSource = null;
            ListViewItem myListViewItem = (ListViewItem)(parameter.inventoryListView.ItemContainerGenerator.ContainerFromItem(itemSelected));
            // Getting the ContentPresenter of myListViewItem
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListViewItem);
            // Finding image from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            Image image = (Image)myDataTemplate.FindName("ImageItem", myContentPresenter);
            image.Source = bitmap;
            screen.ShowDialog();
            if (screen.DialogResult == true)
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
                CopyFileToAppFolder(NewItem.Id.ToString(), _imageFileName);
                InventoryItemsCollection = new CollectionViewSource { Source = _inventoryItems };
                parameter.inventoryListView.ItemsSource = InventorySourceCollection;
            }
            else
            {
            }
            screen.Close();
        }

        private void SaveEditFoodItem(EditFoodItem parameter)
        {
            // 1: Lỗi để trống các textBox
            // 2: Lỗi nhập giá cả âm hoặc ký tự lạ

            int error_type = 0;
            string price = parameter.priceTextBox.Text;
            string name = parameter.nameTextBox.Text;
            string description = parameter.describeTextBox.Text;

            //1. 
            if (price == "" || name == "" || description == "")
            {
                error_type = 1;
            }
            //2.
            else
            {
                float price_test = 0;
                bool check = float.TryParse(price, out price_test);
                if (check == true)
                {
                    price_test = float.Parse(price);
                    if (price_test < 0)
                    {
                        error_type = 2;
                    }
                }
                else error_type = 2;
            }
            switch (error_type)
            {
                case 1:
                    MessageBox.Show("Lỗi! Còn một số trường chưa được nhập giá trị");
                    break;
                case 2:
                    MessageBox.Show("Lỗi! Giá cả nhập vào không hợp lệ");
                    break;
                default:
                    MessageBox.Show("Chỉnh sửa món ăn hàng ngày thành công");
                    parameter.DialogResult = true;
                    break;
            }
        }

        private void EditFoodItem(StorageView parameter)
        {
            _imageFileName = "";
            var itemSelectedidx = parameter.foodListView.SelectedIndex;
            var itemSelected = parameter.foodListView.SelectedItem as Item;
            var screen = new EditFoodItem(itemSelected);
            screen.IdTextBox.Text = itemSelected.Id.ToString();
            screen.nameTextBox.Text = itemSelected.Name;
            screen.priceTextBox.Text = itemSelected.Price.ToString();
            screen.describeTextBox.Text = itemSelected.Description;


            ImageBrush imageBrush = new ImageBrush();
            BitmapImage bitmap = GetBitmap(GetFilePath(itemSelected.Id.ToString()));
            imageBrush.ImageSource = bitmap;
            screen.grdSelectImg.Background = imageBrush;
            screen.imageEmpty.Visibility = Visibility.Hidden;

            // Set temp image
            //parameter.foodListView.ItemsSource = null;
            ListViewItem myListViewItem = (ListViewItem)(parameter.foodListView.ItemContainerGenerator.ContainerFromItem(itemSelected));
            // Getting the ContentPresenter of myListViewItem
            ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListViewItem);
            // Finding image from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            Image image = (Image)myDataTemplate.FindName("ImageItem", myContentPresenter);
            image.Source = bitmap;


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
                _foodItems[itemSelectedidx] = NewItem;
                CopyFileToAppFolder(NewItem.Id.ToString(), _imageFileName);
                FoodItemsCollection = new CollectionViewSource { Source = _foodItems };
                parameter.foodListView.ItemsSource = FoodSourceCollection;
            }
            else
            {
            }

            screen.Close();
        }

        private void DeleteInventoryItem(StorageView parameter)
        {
            int i = parameter.inventoryListView.SelectedIndex;
            DbQueries.ItemQueries.DeleteItemById(_inventoryItems[i].Id);
            _inventoryItems.RemoveAt(i);
        }

        private void DeleteFoodItem(StorageView parameter)
        {
            int i = parameter.foodListView.SelectedIndex;
            /*var itemSelected = parameter.foodListView.SelectedItem as Item;*/
            DbQueries.ItemQueries.DeleteItemById(_foodItems[i].Id);
            _foodItems.RemoveAt(i);
        }

    }
}
