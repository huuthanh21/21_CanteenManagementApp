using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CanteenManagementApp.MVVM.ViewModel.DailyReportModel;

namespace CanteenManagementApp.MVVM.View
{
    public partial class DailyReport : Page
    {
        public static DateTime selectedDate;
        public DailyReport()
        {
            InitializeComponent();
            Day.ItemsSource = LoadComboBoxDate();
            Month.ItemsSource = LoadComboBoxMonth();
            Year.ItemsSource = LoadComboBoxYear();

            selectedDate = DateTime.Today;
            OutputSelectedDate();
        }
        private void ButtonLink_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ReportViewModel.ReportCommand.Execute(sender);
        }

        private void OutputSelectedDate()
        {
            Day.SelectedIndex = (selectedDate.Day - 1);
            Month.SelectedIndex = (selectedDate.Month - 1);
            Year.SelectedIndex = (selectedDate.Year - 2020);
        }
        private static string[] LoadComboBoxMonth()
        {
            string[] MonthArray = new string[12];
            for (int i = 0; i < MonthArray.Length; i++)
            {
                MonthArray[i] = (i + 1).ToString("00");
            }
            return MonthArray;
        }

        private static string[] LoadComboBoxDate()
        {
            string[] DateArray = new string[31];
            for (int i = 0; i < DateArray.Length; i++)
            {
                DateArray[i] = (i + 1).ToString("00");
            }
            return DateArray;
        }

        private static string[] LoadComboBoxYear()
        {
            string[] YearArray = new string[5];
            for (int i = 0; i < YearArray.Length; i++)
            {
                YearArray[i] = (i + 2020).ToString();
            }
            return YearArray;
        }
    }
}