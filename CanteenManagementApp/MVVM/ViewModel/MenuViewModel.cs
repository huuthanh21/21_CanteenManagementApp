using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MenuViewModel : ObservableObject
    {
        public ObservableCollection<Item> FoodItems;
        private readonly CollectionViewSource FoodItemsCollection;
        public ICollectionView FoodSourceCollection => FoodItemsCollection.View;
        public string Keyword { get; set; } = "Fuck";
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

           
        }
    }
}
