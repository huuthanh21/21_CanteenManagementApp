using CanteenManagementApp.MVVM.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for AddInventoryItem.xaml
    /// </summary>
    public partial class AddInventoryItem : Window
    {
        public Item NewItem { get; set; }
        string imageFileName;
        public AddInventoryItem()
        {
            InitializeComponent();
            DataContext = NewItem;
        }
        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            string describe = describeTextBox.Text;
            string price = priceTextBox.Text;
            int amount = int.Parse(amountTextBox.Text);
            NewItem = new Item()
            {
                Name = name,
                Price = float.Parse(price),
                Description = describe,
                Amount = amount,
                ImagePath = imageFileName,
                Type = 1

            };
            await DbQueries.ItemQueries.InsertItemAsync(NewItem);
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
