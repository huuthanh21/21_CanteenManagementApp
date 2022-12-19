using CanteenManagementApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReceiptView.xaml
    /// </summary>
    public partial class ReceiptView : Window
    {
        private readonly CollectionViewSource _totalOrderItemsCollection;

        public Receipt Receipt { get; set; }
        public ObservableCollection<ItemOrder> TotalItemOrder { get; set; }

        public ICollectionView TotalOrderSourceCollection => _totalOrderItemsCollection.View;

        public ReceiptView()
        {
            InitializeComponent();

            DataContext = this;
        }

        public ReceiptView(int receiptId)
        {
            Receipt = DbQueries.ReceiptQueries.GetReceiptById(receiptId);

            if (Receipt.CustomerId == "-1")
            {
                PanelCustomerId.Visibility = Visibility.Collapsed;
            }

            TotalItemOrder = new ObservableCollection<ItemOrder>(DbQueries.ReceiptQueries.GetReceiptDetailsByReceipt(Receipt));

            _totalOrderItemsCollection = new CollectionViewSource { Source = TotalItemOrder };

            InitializeComponent();
            DataContext = this;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    };
}