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

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MenuViewModel : ObservableObject
    {
        public ObservableCollection<Item> FoodItems;
        private readonly CollectionViewSource FoodItemsCollection;
        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public string Keyword { get; set; } = "Fuck";
        public ICommand AddItemToMenuCommand { get; set; }
        public MenuViewModel()
        {
            //FoodItems = new ObservableCollection<Item> { };
           FoodItems = new ObservableCollection<Item>
            {
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000},
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000},
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000 },
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000},
                new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000},
                new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000},
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000 }
            };


            FoodItemsCollection = new CollectionViewSource { Source = FoodItems };

            AddItemToMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => AddItemToMenu(parameter));
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

        private void AddItemToMenu(MenuView parameter)
        {
            //FNL
            //var screen = new AddItemToMenu();
            //screen.ShowDialog();
            //if (screen.DialogResult == true)
            //{var selectedItems = screen.foodListView.SelectedItems;
            //    foreach (var item in selectedItems)
            //    {
            //        ListViewItem myListViewItem = (ListViewItem)(screen.foodListView.ItemContainerGenerator.ContainerFromItem(item));
            //        // Getting the ContentPresenter of myListViewItem
            //        ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListViewItem);

            //        // Finding textBox from the DataTemplate that is set on that ContentPresenter
            //        DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
            //        //TextBlock myTextBlock = (TextBlock)myDataTemplate.FindName("textBlock", myContentPresenter);
            //        TextBox textBox = (TextBox)myDataTemplate.FindName("textBox", myContentPresenter);
            //        Item itemTemp = (Item)item;
            //        itemTemp.Amount = int.Parse(textBox.Text);
            //        //Test:
            //        //MessageBox.Show("The selected item: Name: " + itemTemp.Name + " Amount: " + itemTemp.Amount);
            //        this.FoodItems.Add(itemTemp);
            //    }
            //}
            MessageBox.Show("Today is " + DateTime.Today);
            DateTime yesterday = GetYesterday();
            MessageBox.Show("Yesterday is " + yesterday);
        }
        public static DateTime GetYesterday()
        {
            // Ngày hôm nay.
            DateTime today = DateTime.Today;

            // Trừ đi một ngày.
            return today.AddDays(-1);
        }



    }
}
