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

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MenuViewModel : ObservableObject
    {
        public static ObservableCollection<Item> FoodItemsYesterday;
        public ObservableCollection<Item> FoodItemsToday;
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
            //FoodItems = new ObservableCollection<Item> { };

            FoodItemsYesterday = new ObservableCollection<Item>
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
            FoodItemsToday = new ObservableCollection<Item> {  new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000},
                new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000},
                new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000},
                };

            // Source data:
            FoodItemsYesterdayCollection = new CollectionViewSource { Source = FoodItemsYesterday };
            FoodItemsTodayCollection = new CollectionViewSource { Source = FoodItemsToday };
            ResetAmount(FoodItemsYesterday);
            // Command:
            AddItemToMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => AddItemToMenu(parameter));
            CopyMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => CopyMenu(parameter));
            DeleteItemMenuCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => DeleteItemMenu(parameter));
            UpdateAmountCommand = new RelayCommand<MenuView>((parameter) => true, (parameter) => UpdateAmount(parameter));
            OkUpdateAmount = new RelayCommand<UpdateAmountMenu>((parameter) => true, (parameter) => OkUpdate(parameter));
            CancelUpdateAmount = new RelayCommand<UpdateAmountMenu>((parameter) => true, (parameter) => CancelUpdate(parameter));


            //
            //InvokeSthing(2000);
            //SetDataTimer(5000);
            //SetupData();
        }

        private void CancelUpdate(UpdateAmountMenu parameter)
        {
            parameter.DialogResult = false;
        }

        private void OkUpdate(UpdateAmountMenu parameter)
        {
            parameter.DialogResult = true;
        }

        private void UpdateAmount(MenuView parameter)
        {
            int index = parameter.foodListViewToday.SelectedIndex;
            if (index != -1)
            {
                var screen = new UpdateAmountMenu();
                screen.textBoxAmount.Text = FoodItemsToday[index].Amount.ToString();
                screen.nameItem.Text = FoodItemsToday[index].Name.ToString();
                //screen.ShowDialog();
                if (screen.ShowDialog() == true)
                {
                    FoodItemsToday[index].Amount = int.Parse(screen.textBoxAmount.Text);
                }
                else
                {
                    screen.Close();
                }

            }
        }

        private void DeleteItemMenu(MenuView parameter)
        {
            //MessageBox.Show("DeleteItemMenu");
            int index = parameter.foodListViewToday.SelectedIndex;
            if (index != -1)
            {
                var result = MessageBox.Show($"Bạn muốn xóa mặt hàng {FoodItemsToday[index].Name} khỏi menu",
              "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    FoodItemsToday.RemoveAt(index);
                    //MessageBox.Show("Xoá mặt hàng thành công", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CopyMenu(MenuView parameter)
        {
            FoodItemsToday.Clear();
            foreach (Item item in FoodItemsYesterday)
            {
                FoodItemsToday.Add(item);
            }
        }

        private static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
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
                    //TextBlock myTextBlock = (TextBlock)myDataTemplate.FindName("textBlock", myContentPresenter);
                    TextBox textBox = (TextBox)myDataTemplate.FindName("textBox", myContentPresenter);
                    Item itemTemp = (Item)item;
                    itemTemp.Amount = int.Parse(textBox.Text);
                    //Test:
                    //MessageBox.Show("The selected item: Name: " + itemTemp.Name + " Amount: " + itemTemp.Amount);
                    FoodItemsToday.Add(itemTemp);
                }
            }

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

        public void ResetAmount(ObservableCollection<Item> items)
        {
            foreach (Item item in items)
            {
                item.Amount = 0;
            }
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
        static int Count { get; set; } = 0;

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            ResetAmount(FoodItemsToday);
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
