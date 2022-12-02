using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CanteenManagementApp.Converters
{
    public class FloatToCurrencyConverter : IMultiValueConverter
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
