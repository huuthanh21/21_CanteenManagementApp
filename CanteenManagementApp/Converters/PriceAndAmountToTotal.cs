using System;
using System.Globalization;
using System.Windows.Data;

namespace CanteenManagementApp.Converters
{
    public class PriceAndAmountToTotal : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var total = (float)values[0] * (int)values[1];
            FloatToCurrencyConverter floatToCurrencyConverter = new FloatToCurrencyConverter();
            return floatToCurrencyConverter.Convert(total, targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
