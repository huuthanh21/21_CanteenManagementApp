using System;
using System.Windows;
using System.Windows.Controls;

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