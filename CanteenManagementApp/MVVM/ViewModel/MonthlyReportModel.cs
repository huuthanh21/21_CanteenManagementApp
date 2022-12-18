using CanteenManagementApp.Converters;
using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class MonthlyReportModel : ObservableObject
    {
        public int Month { get; set; }
        public int Year { get; set; }

        private readonly ObservableCollection<ReportItem> _monthlyListItems;

        private readonly CollectionViewSource _reportItemsCollectionSource;
        public ICollectionView ReportItemsCollection => _reportItemsCollectionSource.View;

        public RelayCommand DateChangedCommand { get; set; }

        public MonthlyReportModel()
        {
            _monthlyListItems = new ObservableCollection<ReportItem>
            {
                new ReportItem() { Name = "Tổng doanh thu", Value = "0"},
                new ReportItem() { Name = "Tổng hàng tồn kho bán được", Value = "0"},
                new ReportItem() { Name = "Tổng doanh thu hàng tồn kho", Value = "0"},
                new ReportItem() { Name = "Tổng món ăn hàng ngày bán được", Value = "0"},
                new ReportItem() { Name = "Tổng doanh thu món ăn hàng ngày", Value = "0"}
            };

            _reportItemsCollectionSource = new CollectionViewSource { Source = _monthlyListItems };

            DateChangedCommand = new RelayCommand(async o =>
            {
                await UpdateUI();
            });

            _ = UpdateUI();
        }

        private async Task UpdateUI()
        {
            DateOnly date;
            if (Month == 0)
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            else
            {
                date = new(Year, Month, 1);
            }
            await UpdateReportRevenue(date);
            await UpdateReportSales(date);
        }

        private async Task UpdateReportRevenue(DateOnly date)
        {
            FloatToCurrencyConverter floatToCurrencyConverter = new();

            // Tổng doanh thu
            _monthlyListItems[0].Value = floatToCurrencyConverter.Convert(await DbQueries.ReceiptQueries.GetMonthRevenueAsync(date),
                                                                            null, null, new CultureInfo("vi-VN")) as string;
        }

        private async Task UpdateReportSales(DateOnly date)
        {
            var foodSales = await DbQueries.ReceiptQueries.GetItemSalesByMonthAsync(date, 0); // Món ăn
            var otherSales = await DbQueries.ReceiptQueries.GetItemSalesByMonthAsync(date, 1); // Hàng tồn

            FloatToCurrencyConverter floatToCurrencyConverter = new();

            // Cập nhật dữ liệu hàng tồn kho
            _monthlyListItems[1].Value = otherSales.Item1.ToString(); // Doanh số
            _monthlyListItems[2].Value = floatToCurrencyConverter.Convert(otherSales.Item2, null, null,
                                                                            new CultureInfo("vi-VN")) as string; // Doanh thu

            // Cập nhật dữ liệu món ăn
            _monthlyListItems[3].Value = foodSales.Item1.ToString(); // Doanh số
            _monthlyListItems[4].Value = floatToCurrencyConverter.Convert(foodSales.Item2, null, null,
                                                                            new CultureInfo("vi-VN")) as string; // Doanh thu
        }
    }
}