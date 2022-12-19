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
using System.Timers;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

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
        public RelayCommand AddItemToMenuCommand { get; set; }
        public ICommand DeleteItemMenuCommand { get; set; }

        public RelayCommand CopyMenuCommand { get; set; }

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
            _ = ResetAmount(_foodItemsYesterday.Except(_foodItemsToday, new ItemComparer()));

            // Command:
            AddItemToMenuCommand = new RelayCommand(async o => await AddItemToMenu());
            CopyMenuCommand = new RelayCommand(async o => await CopyMenu());
            DeleteItemMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => DeleteItemMenu(parameter));
            UpdateAmountCommand = new RelayCommand<MenuView>((parameter) => true, async (parameter) => await UpdateAmount(parameter));
            OkUpdateAmount = new RelayCommand<UpdateAmountMenu>((parameter) => true, (parameter) => OkUpdate(parameter));
            CancelUpdateAmount = new RelayCommand<UpdateAmountMenu>((parameter) => true, (parameter) => CancelUpdate(parameter));

            //
            //InvokeSthing(2000);
            //SetDataTimer(5000);
            SetupData();
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
                    // template
                    var amount_template = screen.textBoxAmount.Template;
                    var control_amount = (TextBox)amount_template.FindName("TextboxInput", screen.textBoxAmount);

                    //int amount = int.Parse(screen.textBoxAmount.Text);
                    int amount = int.Parse(control_amount.Text);
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
                    _foodItemsToday[index].Amount = 0;
                    _foodItemsToday.RemoveAt(index);
                }

                // Update database
                DbQueries.MenuQueries.DeleteMenuItem(item);
                _ = DbQueries.ItemQueries.UpdateItem(item);
            }
        }

        private async Task CopyMenu()
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

        private static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
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

        private async Task AddItemToMenu()
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
                    // template
                    var amount_template = textBox.Template;
                    var control_amount = (TextBox)amount_template.FindName("TextboxInput", textBox);
                    string amount = control_amount.Text;

                    //end template

                    //Test time:
                    /* DateTime today = DateTime.Today;*/
                    //DateTime today = DateTime.Now;
                    //MessageBox.Show("Now is " + today);
                    //DateTime yesterday = GetYesterday();
                    //DateTime tomorrow = GetTomorrow();
                    //MessageBox.Show("Yesterday is " + yesterday + "\nToday is " + tomorrow);
                    //TimeSpan interval = tomorrow.Subtract(today);
                    //MessageBox.Show("From today to tomorrow is: " + interval.Hours * 60 + interval.Minutes * 60 + interval.Seconds + " s");
                    //*60 + interval.Minutes * 60 + interval.Seconds + " s"

                    Item newItem = (Item)item;
                    //newItem.Amount = int.Parse(textBox.Text);
                    newItem.Amount = int.Parse(amount);
                    _foodItemsToday.Add(newItem);
                    await DbQueries.MenuQueries.InsertMenuItem(newItem);
                }
            }
        }

        public static DateTime GetYesterday()
        {
            // Ngày hôm nay.
            DateTime today = DateTime.Today;

            // Trừ đi một ngày.
            return today.AddDays(-1);
        }

        public static DateTime GetTomorrow()
        {
            // Ngày hôm nay.
            DateTime today = DateTime.Today;

            return today.AddDays(1);
        }

        public static async Task ResetAmount(IEnumerable<Item> items)
        {
            // Check if items is empty
            if (!items.Any()) return;

            foreach (Item item in items)
            {
                item.Amount = 0;
            }
            await DbQueries.MenuQueries.ResetAmountMenuItems(items);
        }

        public static void InvokeSthing(int sleepTime)
        {
            // command block goes here!
            DateTime today = DateTime.Now;
            MessageBox.Show("Now is " + today);
            Thread.Sleep(sleepTime);
            today = DateTime.Now;
            MessageBox.Show("Now is " + today);
        }

        private Timer aTimer;

        public void SetDataTimer(int milliSeconds)
        {
            aTimer = new System.Timers.Timer();
            aTimer.Interval = milliSeconds;
            // Hook up the Elapsed event for the timer.
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = false;

            // Start the timer
            aTimer.Enabled = true;

            //MessageBox.Show("Press the Enter key to exit the program at any time... ");
            //Console.WriteLine("Press the Enter key to exit the program at any time... ");
            //Console.ReadLine();
        }

        private static int Count { get; set; } = 0;

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            _ = ResetAmount(_foodItemsToday);
            MessageBox.Show("The Elapsed event was raised at " + e.SignalTime.ToString() + " count: " + Count.ToString());
            Count++;
            //if (Count == 2 || Count > 5)
            //{
            //    aTimer.Stop();
            //    aTimer.AutoReset = false;
            //    aTimer.Enabled=false;
            //    aTimer.EndInit();
            //}
            SetupData();
        }

        public void SetupData()
        {
            //
            //Test time:
            /* DateTime today = DateTime.Today;*/
            //DateTime today = DateTime.Now;
            //MessageBox.Show("Now is " + today);
            //DateTime yesterday = GetYesterday();
            //DateTime tomorrow = GetTomorrow();
            //MessageBox.Show("Yesterday is " + yesterday + "\nToday is " + tomorrow);
            //TimeSpan interval = tomorrow.Subtract(today);
            //MessageBox.Show("From today to tomorrow is: " + interval.Hours * 60 + interval.Minutes * 60 + interval.Seconds + " s");
            //*60 + interval.Minutes * 60 + interval.Seconds + " s"
            //SetDataTimer(interval);
            if (Count == 0)
            {
                SetDataTimer(5000);
            }
        }
    }
}