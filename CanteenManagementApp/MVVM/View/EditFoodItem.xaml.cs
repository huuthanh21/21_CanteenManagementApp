﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CanteenManagementApp.MVVM.Model;
using Microsoft.Win32;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditFoodItem : Window
    {
        //public Item EditedItem { get; set; }
        public EditFoodItem(Item item)
        {
            InitializeComponent();
            //EditedItem = (Item)item.Clone();
            //DataContext = EditedItem;
        }


       /* private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }*/

        //private void ChangeImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog op = new()
        //    {
        //        Title = "Chọn ảnh",
        //        Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
        //    };
        //    if (op.ShowDialog() == true)
        //    {
        //        string imageFileName = op.FileName;
        //        ImageBrush imageBrush = new();
        //        BitmapImage bitmap = new();
        //        bitmap.BeginInit();
        //        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmap.UriSource = new Uri(imageFileName);
        //        bitmap.EndInit();
        //        imageBrush.ImageSource = bitmap;
        //        grdSelectImg.Background = imageBrush;
        //        //imageEmpty.Visibility = Visibility.Hidden;
        //        EditedItem.ImagePath = imageFileName;
        //    }
        //}
    }
}
