using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace CanteenManagementApp.Converters
{
    public class FloatToCurrencyConverter : IMultiValueConverter, IValueConverter
    {
        // values[0]: float, parameter: Button.Tag
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            decimal dec;
            try
            {
                dec = new((float)values[0]);
            }
            catch (InvalidCastException)
            {
                return "";
            }
            string currency = string.Format(culture, "{0:c}", dec);

            // Password is not showing
            if (!(bool)values[1])
            {
                return string.Concat(Enumerable.Repeat("*", currency.Length));
            }

            return currency;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((parameter as string) == "Tổng hàng tồn kho bán được" || (parameter as string) == "Tổng món ăn hàng ngày bán được")
            {
                return value;
            }

            decimal dec;
            string currency;
            try
            {
                dec = new((float)value);
            }
            catch (InvalidCastException)
            {
                dec = new decimal(0);
                currency = string.Format(culture, "{0:c}", dec);

                return currency;
            }
            currency = string.Format(culture, "{0:c}", dec);

            return currency;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}