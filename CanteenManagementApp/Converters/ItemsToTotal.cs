using CanteenManagementApp.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace CanteenManagementApp.Converters
{
    public class ItemsToTotal : IValueConverter
    {
        // ICollectionView
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float total = 0;
            foreach(ItemOrder item in (ObservableCollection<ItemOrder>)value)
            {
                total += (item.Item.Price * item.Amount);
            }
            FloatToCurrencyConverter floatToCurrencyConverter = new();
            return floatToCurrencyConverter.Convert(total, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
