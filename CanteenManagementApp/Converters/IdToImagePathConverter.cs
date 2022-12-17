using CanteenManagementApp.MVVM.View;
using CanteenManagementApp.MVVM.ViewModel;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;

namespace CanteenManagementApp.Converters
{
    public class IdToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = "21CanteenManager";

            var appPath = Path.Combine(appdataPath, appFolder);
            StringBuilder stringBuilder = new();
            stringBuilder.Append(value.ToString());
            stringBuilder.Append(".jpg");

            var filePath = Path.Combine(appPath, stringBuilder.ToString());
            bool fileExist = File.Exists(filePath);
            if (fileExist)
            {
                return filePath;
            }

            var defaultImageName = "default.jpg";
            var defaultImagePath = Path.Combine(appPath, defaultImageName);

            return defaultImagePath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
