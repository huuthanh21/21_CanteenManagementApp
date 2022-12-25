using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class AddItemToMenuViewModel : ObservableObject
    {
        public ObservableCollection<Item> FoodItems { get; set; }
        private readonly CollectionViewSource FoodItemsCollection;
        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public ICommand AddItemToMenuOkCommand { get; set; } // nút thêm hàng vào menu
        public ICommand CancelAddItemToMenuCommand { get; set; }                      //
        public ObservableCollection<Item> FoodItemsOld { get; set; }
        public AddItemToMenuViewModel()
        {

            FoodItems = new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(0));
            //{
            //    //new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 12000},
            //    //new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000},
            //    //new Item() { Type = 0, Name = "Lòng heo xào gà", Amount = 10, Description = "Ngon", Id = 12, Price = 12000},
            //    //new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 13, Price = 12000 },
            //    //new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 14, Price = 12000},
            //    //new Item() { Type = 0, Name = "Lòng xào gà", Amount = 10, Description = "Ngon", Id = 15, Price = 12000},
            //    //new Item() { Type = 0, Name = "Gà nướng", Amount = 10, Description = "Ngon", Id = 10, Price = 12000},
            //    //new Item() { Type = 0, Name = "Lòng bò xào gà", Amount = 10, Description = "Ngon", Id = 11, Price = 12000 }
            //};

            //using var dbContext = new CanteenContext();
            //var items = dbContext.Items;
            //foreach (var item in items)
            //{
            //    FoodItems.Add(item);
            //}

           FoodItemsOld = new ObservableCollection<Item>(DbQueries.MenuQueries.GetTodayMenuItems());


            for (int i = 0; i < FoodItemsOld.Count; i++)
            {
                for (int j = 0; j < FoodItems.Count; j++)
                {
                    if (FoodItemsOld[i].Name.Equals(FoodItems[j].Name)
                        && FoodItemsOld[i].Price == FoodItems[j].Price
                        && FoodItemsOld[i].Description.Equals(FoodItems[j].Description))
                    {
                        FoodItems.RemoveAt(j);
                    }
                }
            }

            FoodItemsCollection = new CollectionViewSource { Source = FoodItems };
            FoodSourceCollection.Filter = ItemFilter;
            AddItemToMenuOkCommand = new RelayCommand<AddItemToMenu>((parameter) => true, (parameter) => AddItemToMenuOk(parameter));
            CancelAddItemToMenuCommand = new RelayCommand<AddItemToMenu>((parameter) => true, (parameter) => CancelAddItemToMenu(parameter));
            TextSearchBarChanged += RefreshItemViewSource;
        }

        private void CancelAddItemToMenu(AddItemToMenu parameter)
        {
            parameter.DialogResult = false;
        }

        private object _textSearchBar;

        private readonly EventHandler TextSearchBarChanged;
        public object TextSearchBar
        {
            get { return _textSearchBar; }
            set
            {
                _textSearchBar = value;
                OnTextSearchBarChanged(EventArgs.Empty);
            }
        }
        public bool ItemFilter(object item)
        {
            if (string.IsNullOrEmpty(TextSearchBar as string))
            {
                return true;
            }

            return (item as Item).Name.Contains(TextSearchBar as string, StringComparison.OrdinalIgnoreCase);
        }

        private void RefreshItemViewSource(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(FoodSourceCollection).Refresh();
        }

        [SuppressPropertyChangedWarnings]
        public void OnTextSearchBarChanged(EventArgs e)
        {
            TextSearchBarChanged?.Invoke(this, e);
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

        public void AddItemToMenuOk(AddItemToMenu parameter)
        {
            var selectedItems = parameter.foodListView.SelectedItems;
            bool check = true;
            foreach (var item in selectedItems)
            {
                ListViewItem myListViewItem = (ListViewItem)(parameter.foodListView.ItemContainerGenerator.ContainerFromItem(item));
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

                int amount_test = 0;
                check = int.TryParse(amount, out amount_test);
                if (check && int.Parse(amount) >= 0)
                {
                }
                else
                {
                    MessageBox.Show("Lỗi! Số lượng món ăn hàng ngày được nhập không hợp lệ");
                    return;
                }

            }

            parameter.DialogResult = true;
        }
      
    }


}
