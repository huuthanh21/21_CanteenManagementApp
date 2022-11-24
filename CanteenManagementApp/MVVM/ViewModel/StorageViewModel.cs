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
    public class StorageViewModel: INotifyPropertyChanged
    {
        private readonly CollectionViewSource StorageItemsCollection;
        public ICollectionView StorageSourceCollection => StorageItemsCollection.View;
        public StorageViewModel()
        {
            var _allItems = new ObservableCollection<Book>
            {
                new Book() { Name = "In Search of Lost Time", Author="Marcel Proust",  ImagePath= "/Images/icon_report.png", PublishedYear = "1987"},
                new Book() { Name = "Ulysses", Author="James Joyce",  ImagePath= "/Images/b2.jpg", PublishedYear = "1977"},
                new Book() { Name = "Don Quixote", Author="Miguel de Cervantes",  ImagePath="/Images/b3.jpg", PublishedYear = "1968"},
                new Book() { Name = "The Great Gatsby", Author="F.Scott Fitzgerald",  ImagePath= "Images/b4.jpg", PublishedYear = "1987"},
                new Book() { Name = "Moby Dick", Author="Herman Melville",  ImagePath= "Images/b5.jpg", PublishedYear = "1986"},
                new Book() { Name = "War and Peace", Author="Leo Tolstoy",  ImagePath= "Images/b6.jpg", PublishedYear = "1991"},
                new Book() { Name = "Hamlet", Author="William Shakespeare",  ImagePath= "Images/b7.jpg", PublishedYear = "1987"},
                new Book() { Name = "The Odyssey", Author="Homer",  ImagePath= "Images/b8.jpg", PublishedYear = "1988"},
                new Book() { Name = "Madame Bovary", Author="Gustave Flaubert",  ImagePath="Images/b9.jpg", PublishedYear = "1897"},
                new Book() { Name = "The Divine Comedy", Author="Dante Alighieri",  ImagePath="Images/b10.jpg", PublishedYear = "1982"},
                new Book() { Name = "In Search of Lost Time", Author="Marcel Proust",  ImagePath="Images/b10.jpg", PublishedYear = "1987"},
           };
            StorageItemsCollection = new CollectionViewSource {  Source = _allItems };
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
