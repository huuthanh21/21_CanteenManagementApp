using CanteenManagementApp.Converters;
using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.View;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class DailyReportModel : ObservableObject
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        private readonly CollectionViewSource ReportItemsCollectionSource;
        public ICollectionView ReportItemsCollection => ReportItemsCollectionSource.View;

        public RelayCommand DateChangedCommand { get; set; }

        private readonly ObservableCollection<ReportItem> _dailyListItems;

        public DailyReportModel()
        {
            _dailyListItems = new()
            {
                new ReportItem() { Name = "Tổng doanh thu", Value = "0"},
                new ReportItem() { Name = "Tổng hàng tồn kho bán được", Value = "0"},
                new ReportItem() { Name = "Tổng doanh thu hàng tồn kho", Value = "0"},
                new ReportItem() { Name = "Tổng món ăn hàng ngày bán được", Value = "0"},
                new ReportItem() { Name = "Tổng doanh thu món ăn hàng ngày", Value = "0"}
            };
            ReportItemsCollectionSource = new CollectionViewSource { Source = _dailyListItems };

            DateChangedCommand = new RelayCommand(async o =>
            {
                await UpdateUI();
            });

            _ = UpdateUI();
        }

        private async Task UpdateUI()
        {
            DateOnly date;
            if (Day == 0)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                date = new(Year, Month, Day);
            }
            await UpdateReportRevenue(date);
            await UpdateReportSales(date);
        }

        private async Task UpdateReportRevenue(DateOnly date)
        {
            FloatToCurrencyConverter floatToCurrencyConverter = new();

            // Tổng doanh thu
            _dailyListItems[0].Value = floatToCurrencyConverter.Convert(await DbQueries.ReceiptQueries.GetDayRevenueAsync(date),
                                                                            null, null, new CultureInfo("vi-VN")) as string;
        }

        private async Task UpdateReportSales(DateOnly date)
        {
            var foodSales = await DbQueries.ReceiptQueries.GetItemSalesByDayAsync(date, 0); // Món ăn
            var otherSales = await DbQueries.ReceiptQueries.GetItemSalesByDayAsync(date, 1); // Hàng tồn

            FloatToCurrencyConverter floatToCurrencyConverter = new();

            // Cập nhật dữ liệu hàng tồn kho
            _dailyListItems[1].Value = otherSales.Item1.ToString(); // Doanh số
            _dailyListItems[2].Value = floatToCurrencyConverter.Convert(otherSales.Item2, null, null,
                                                                            new CultureInfo("vi-VN")) as string; // Doanh thu

            // Cập nhật dữ liệu món ăn
            _dailyListItems[3].Value = foodSales.Item1.ToString(); // Doanh số
            _dailyListItems[4].Value = floatToCurrencyConverter.Convert(foodSales.Item2, null, null,
                                                                            new CultureInfo("vi-VN")) as string; // Doanh thu
        }
    }
}