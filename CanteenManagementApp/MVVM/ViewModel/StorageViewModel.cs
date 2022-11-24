using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class StorageViewModel : ObservableObject
    {
        private readonly CollectionViewSource StorageItemsCollection;
        public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        public StorageViewModel()
        {
            var _allItems = new ObservableCollection<Item>
            {
                new Item() { Name = "Gà nướng", Amount = 10, Describe = "Ngon", Id = 10, Price = 120000, ImagePath = "/Images/b1.jpg" },
                new Item() { Name = "Lòng bò xào gà", Amount = 10, Describe = "Ngon", Id = 11, Price = 120000,  ImagePath = "/Images/b2.jpg" },
                new Item() { Name = "Lòng heo xào gà", Amount = 10, Describe = "Ngon", Id = 12, Price = 120000 ,  ImagePath = "/Images/b3.jpg"},
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 13, Price = 120000,  ImagePath = "/Images/b4.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 14, Price = 120000,  ImagePath = "/Images/b5.jpg" },
                new Item() { Name = "Lòng xào gà", Amount = 10, Describe = "Ngon", Id = 15, Price = 120000,  ImagePath = "/Images/b1.jpg" },

            };
            StorageItemsCollection = new CollectionViewSource {  Source = _allItems };
        }
    }
}
