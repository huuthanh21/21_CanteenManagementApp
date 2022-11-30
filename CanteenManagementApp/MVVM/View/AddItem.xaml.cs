using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CanteenManagementApp.MVVM.Model;
using Microsoft.Win32;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        public Item NewItem { get; set; }
        string _imageFileName = "/Images/empty_image.jpg";
        int _type;
        public AddItem(int type)
        {
            InitializeComponent();
            DataContext = NewItem;
            if (type == 0)
            {
                titleSub.Text = "Món ăn hàng ngày";
            }
            else
            {
                titleSub.Text = "Hàng tồn kho";
            }
            _type = type;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string id = IdTextBox.Text;
            string name = nameTextBox.Text;
            string description = describeTextBox.Text;
            string price = priceTextBox.Text;
            NewItem = new Item()
            {
                Id = int.Parse(id),
                Name = name,
                Price = float.Parse(price),
                Description = description,
                Amount = 0,
                ImagePath = _imageFileName,
                Type = _type
            };
            DialogResult = true;
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Chọn ảnh";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(_imageFileName);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                grdSelectImg.Background = imageBrush;
                imageEmpty.Visibility = Visibility.Hidden;

            }
        }
    }
}
