using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MonthlyReportModel : ObservableObject
    {
        public ObservableCollection<ReportItem> monthlyListItems;

        private CollectionViewSource ReportItemsCollectionSource;
        public ICollectionView ReportItemsCollection => ReportItemsCollectionSource.View;

        public MonthlyReportModel()
        {
            monthlyListItems = new ObservableCollection<ReportItem>
            {
                new ReportItem() { Name = "Tổng doanh thu", Value = "0"},
                new ReportItem() { Name = "Tổng hàng tồn kho bán được", Value = "0"},
                new ReportItem() { Name = "Tổng doanh thu hàng tồn kho", Value = "0"},
                new ReportItem() { Name = "Tổng món ăn hàng ngày bán được", Value = "0"},
                new ReportItem() { Name = "Tổng doanh thu món ăn hàng ngày", Value = "0"}
            };

            ReportItemsCollectionSource = new CollectionViewSource { Source = monthlyListItems };
        }
    }
}