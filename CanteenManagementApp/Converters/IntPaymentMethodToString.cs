﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace CanteenManagementApp.Converters
{
    public class IntPaymentMethodToString : IValueConverter
    {
        // value: PayInCash
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if pay in cash
            return (bool)value ? "Tiền mặt" : "Trả trước";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
