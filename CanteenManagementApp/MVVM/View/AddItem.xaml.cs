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
        string imageFileName;
        public AddItem()
        {
            InitializeComponent();
            DataContext = NewItem;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string id = IdTextBox.Text;
            string name = IdTextBox.Text;
            string describe = describeTextBox.Text;
            string price = priceTextBox.Text;
            NewItem = new Item()
            {
                Id = int.Parse(id),
                Name = name,
                Price = float.Parse(price),
                Description = describe,
                Amount = 0,
                ImagePath = imageFileName,
                Type = 0

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
                imageFileName = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imageFileName);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                grdSelectImg.Background = imageBrush;
            }
        }
    }
}
