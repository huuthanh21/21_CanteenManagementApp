using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        public ObservableCollection<Item> _allItems;
        private CollectionViewSource StorageItemsCollection;
        public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        public ICommand EditItemCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public StorageViewModel()
        {
            _allItems = new ObservableCollection<Item>
            {
                new Item() { Name = "Gà nướng", Amount = 10, Describe = "Món ức gà chiên mắm cay có màu sắc hấp dẫn, vị mặn mặn chua chua của nước sốt giúp thịt gà tăng thêm hương vị và kích thích vị giác người ăn. vị đậm đà của mùi nước mắm quyện vào thịt, và vị bùi bùi của tỏi, cay nồng của ớt sừng. Món này ăn kèm cơm nóng, hoặc bánh mì tùy thích.\r\n\r\n\r\n", Id = 10, Price = 120000, ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 120000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 120000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 120000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 120000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 120000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 120000, ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 120000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 120000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 120000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 120000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 120000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 120000, ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 120000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 120000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 120000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 120000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 120000,  ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 120000, ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 120000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 120000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 120000,  ImagePath = "/Images/b4.jpg" },
                
              

            };
            StorageItemsCollection = new CollectionViewSource {  Source = _allItems };
            // ViewBillCommand = new RelayCommand<PayWindow>((parameter) => true, (parameter) => ViewBill(parameter));
            //EditItemCommand = new RelayCommand(StorageView =>  ShowEditItem(StorageView) );
            EditItemCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => EditItem(parameter));
            DeleteCommand = new RelayCommand<StorageView>((parameter) => true, (parameter) => deleteItem(parameter));
        }

        private void EditItem(StorageView parameter)
        {
            var itemSelected = parameter.foodListView.SelectedItem as Item;
            var screen = new EditItem(itemSelected);
            //screen.Show();
            if (screen.ShowDialog() == true)
            {
                _allItems[parameter.foodListView.SelectedIndex] = screen.EditedItem;
            }
            else
            {
                

            }
            screen.Close();
        }

        private void deleteItem(StorageView parameter)
        {
            int i = parameter.foodListView.SelectedIndex;
            _allItems.RemoveAt(i);
        }

       

       
    }
}
