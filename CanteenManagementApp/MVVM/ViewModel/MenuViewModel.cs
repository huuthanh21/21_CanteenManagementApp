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
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MenuViewModel : ObservableObject
    {
        private readonly ObservableCollection<Item> _foodItemsYesterday;
        private readonly ObservableCollection<Item> _foodItemsToday;
        private readonly CollectionViewSource FoodItemsYesterdayCollection;
        private readonly CollectionViewSource FoodItemsTodayCollection;
        public ICollectionView FoodYesterdaySourceCollection => FoodItemsYesterdayCollection.View;
        public ICollectionView FoodTodaySourceCollection => FoodItemsTodayCollection.View;
        public string Keyword { get; set; } = "Fuck";
        public ICommand AddItemToMenuCommand { get; set; }
        public ICommand DeleteItemMenuCommand { get; set; }

        public ICommand CopyMenuCommand { get; set; }

        // Update amount item:
        public ICommand UpdateAmountCommand { get; set; }

        public ICommand OkUpdateAmount { get; set; }
        public ICommand CancelUpdateAmount { get; set; }

        public MenuViewModel()
        {
            _foodItemsYesterday = new ObservableCollection<Item>(DbQueries.MenuQueries.GetYesterdayMenuItems());
            _foodItemsToday = new ObservableCollection<Item>(DbQueries.MenuQueries.GetTodayMenuItems());

            // Source data:
            FoodItemsYesterdayCollection = new CollectionViewSource { Source = _foodItemsYesterday };
            FoodItemsTodayCollection = new CollectionViewSource { Source = _foodItemsToday };
            _ = ResetAmount(_foodItemsYesterday);

            // Command:
            AddItemToMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => AddItemToMenu(parameter));
            CopyMenuCommand = new RelayCommand<MenuView>((parameter) => true, async (parameter) => await CopyMenu(parameter));
            DeleteItemMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => DeleteItemMenu(parameter));
            UpdateAmountCommand = new RelayCommand<MenuView>((parameter) => true, async (parameter) => await UpdateAmount(parameter));
            OkUpdateAmount = new RelayCommand<UpdateAmountMenu>((parameter) => true, (parameter) => OkUpdate(parameter));
            CancelUpdateAmount = new RelayCommand<UpdateAmountMenu>((parameter) => true, (parameter) => CancelUpdate(parameter));
        }

        private static void CancelUpdate(UpdateAmountMenu parameter)
        {
            parameter.DialogResult = false;
        }

        private static void OkUpdate(UpdateAmountMenu parameter)
        {
            parameter.DialogResult = true;
        }

        private async Task UpdateAmount(MenuView parameter)
        {
            int index = parameter.foodListViewToday.SelectedIndex;
            if (index != -1)
            {
                var screen = new UpdateAmountMenu();
                screen.textBoxAmount.Text = _foodItemsToday[index].Amount.ToString();
                screen.nameItem.Text = _foodItemsToday[index].Name.ToString();
                if (screen.ShowDialog() == true)
                {
                    int amount = int.Parse(screen.textBoxAmount.Text);
                    _foodItemsToday[index].Amount = amount;
                    await DbQueries.MenuQueries.UpdateAmountMenuItem(_foodItemsToday[index], amount);
                }
                else
                {
                    screen.Close();
                }
            }
        }

        private void DeleteItemMenu(MenuView parameter)
        {
            int index = parameter.foodListViewToday.SelectedIndex;
            if (index != -1)
            {
                var item = _foodItemsToday[index];
                var result = MessageBox.Show($"Bạn muốn xóa mặt hàng {item.Name}",
                                                "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _foodItemsToday.RemoveAt(index);
                }

                // Update database (bug)
                //DbQueries.MenuQueries.DeleteMenuItem(item);

                //  unprofessional :V
                _ = DbQueries.MenuQueries.InsertMenuItems(_foodItemsToday);
            }
        }

        private async Task CopyMenu(MenuView parameter)
        {
            // Update UI
            _foodItemsToday.Clear();
            foreach (Item item in _foodItemsYesterday)
            {
                _foodItemsToday.Add(item);
            }

            // Update database
            await DbQueries.MenuQueries.InsertMenuItems(_foodItemsYesterday);
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is childItem item)
                {
                    return item;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void AddItemToMenu(MenuView parameter)
        {
            //FNL
            var screen = new AddItemToMenu();
            screen.ShowDialog();
            if (screen.DialogResult == true)
            {
                var selectedItems = screen.foodListView.SelectedItems;
                foreach (var item in selectedItems)
                {
                    ListViewItem myListViewItem = (ListViewItem)(screen.foodListView.ItemContainerGenerator.ContainerFromItem(item));
                    // Getting the ContentPresenter of myListViewItem
                    ContentPresenter myContentPresenter = FindVisualChild<ContentPresenter>(myListViewItem);

                    // Finding textBox from the DataTemplate that is set on that ContentPresenter
                    DataTemplate myDataTemplate = myContentPresenter.ContentTemplate;
                    TextBox textBox = (TextBox)myDataTemplate.FindName("textBox", myContentPresenter);
                    Item itemTemp = (Item)item;
                    itemTemp.Amount = int.Parse(textBox.Text);
                    _foodItemsToday.Add(itemTemp);
                }

            }
            _ = DbQueries.MenuQueries.InsertMenuItems(_foodItemsToday);
        }

        public static DateTime GetYesterday()
        {
            // Ngày hôm nay.
            DateTime today = DateTime.Today;

            // Trừ đi một ngày.
            return today.AddDays(-1);
        }

        public static async Task ResetAmount(ObservableCollection<Item> items)
        {
            foreach (Item item in items)
            {
                item.Amount = 0;
            }
            await DbQueries.MenuQueries.UpdateAmountMenuItems(items);
        }
    }
}