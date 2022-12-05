using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using CanteenManagementApp.MVVM.Model;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    /// 
    public partial class MenuView : UserControl
    {
        public ObservableCollection<Item> FoodItems;
        public MenuView()
        {
            InitializeComponent();
            //FoodItems = new ObservableCollection<Item>();
            //var var2 = foodListView.SelectedIndex;
            //foreach (var item in foodListView.SelectedItems)
            //{
            //    var2 = foodListView.Items.IndexOf(foodListView.SelectedItems[0]);
            //    string a = (foodListView.ItemTemplate.FindName("textBox",))
            //}
           
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = foodListView.SelectedItems;

            
            foreach(var item in selectedItems)
            { 
                ListViewItem myListViewItem = (ListViewItem)(foodListView.ItemContainerGenerator.ContainerFromItem(item));

                // Getting the ContentPresenter of myListBoxItem
                ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListViewItem);

                // Finding textBlock from the DataTemplate that is set on that ContentPresenter
                DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                //TextBlock myTextBlock = (TextBlock)myDataTemplate.FindName("textBlock", myContentPresenter);
                TextBox textBox = (TextBox)myDataTemplate.FindName("textBox", myContentPresenter);
                MessageBox.Show("The text of the TextBox of the selected list item: " + textBox.Text);
            }
           
        }
    }
}
