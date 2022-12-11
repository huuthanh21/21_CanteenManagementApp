using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CanteenManagementApp.MVVM.Model;
using Microsoft.Win32;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddFoodItem.xaml
    /// </summary>
    public partial class AddFoodItem : Window
    {
        public AddFoodItem()
        {
            InitializeComponent();
        }
        //public Item NewItem { get; set; }
        //string _imageFileName = "/Images/empty_image.jpg";
        //int _type;
        //public AddItem(int type)
        //{
        //    InitializeComponent();
        //    DataContext = NewItem;
        //    if (type == 0)
        //    {
        //        titleSub.Text = "Món ăn hàng ngày";
        //    }
        //    else
        //    {
        //        titleSub.Text = "Hàng tồn kho";
        //    }
        //    _type = type;
        //}

        //async private void AddButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string name = nameTextBox.Text;
        //    string description = describeTextBox.Text;
        //    string price = priceTextBox.Text;
        //    NewItem = new Item()
        //    {
        //        Name = name,
        //        Price = float.Parse(price),
        //        Description = description,
        //        Amount = 0,
        //        Type = _type
        //    };

        //    await DbQueries.ItemQueries.InsertItemAsync(NewItem);

        //    CopyFileToAppFolder(NewItem.Id.ToString(), _imageFileName);
        //    DialogResult = true;
        //}

        //private void CopyFileToAppFolder(string id, string sourceFileName)
        //{
        //    var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //    var appFolder = "21CanteenManager";

        //    var appPath = Path.Combine(appdataPath, appFolder);
        //    StringBuilder stringBuilder = new();
        //    stringBuilder.Append(id);
        //    stringBuilder.Append(".jpg");

        //    var filePath = Path.Combine(appPath, stringBuilder.ToString());

        //    File.Copy(sourceFileName, filePath);
        //}

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
        //        grdSelectImg.Background = imageBrush;
        //        imageEmpty.Visibility = Visibility.Hidden;
        //    }
        //}

    }
}
