using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace CanteenManagementApp.MVVM.ViewModel
{

    public class DailyReportModel : ObservableObject
    {
        public ObservableCollection<ReportItem> dailyListItems;

        private CollectionViewSource ReportItemsCollectionSource;
        public ICollectionView ReportItemsCollection => ReportItemsCollectionSource.View;
        
        public DailyReportModel()
        {
            dailyListItems = new ObservableCollection<ReportItem>
            {
                new ReportItem() { Name = "Tổng doanh thu", Value = "1.050.000đ"},
                new ReportItem() { Name = "Tổng hàng tồn kho bán được", Value = "45"},
                new ReportItem() { Name = "Tổng doanh thu hàng tồn kho", Value = "450.000đ"},
                new ReportItem() { Name = "Tổng món ăn hàng ngày bán được", Value = "25"},
                new ReportItem() { Name = "Tổng doanh thu món ăn hàng ngày", Value = "600.000đ"},
            };

            ReportItemsCollectionSource = new CollectionViewSource { Source = dailyListItems };
        }
    }
}
