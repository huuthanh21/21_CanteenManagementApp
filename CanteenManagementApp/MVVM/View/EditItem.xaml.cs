using System.Windows;
using CanteenManagementApp.MVVM.Model;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditItem.xaml
    /// </summary>
    public partial class EditItem : Window
    {
        public Item EditedItem { get; set; }
        public EditItem(Item item)
        {
            InitializeComponent();
            EditedItem = (Item)item.Clone();
            DataContext = EditedItem;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
