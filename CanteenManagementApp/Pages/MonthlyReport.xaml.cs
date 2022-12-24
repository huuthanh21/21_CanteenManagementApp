using System;
using System.Windows;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    public partial class MonthlyReport : Page
    {
        public static DateTime selectedDate;
        public MonthlyReport()
        {
            InitializeComponent();
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

        private static string[] LoadComboBoxYear()
        {
            string[] YearArray = new string[5];
            for (int i = 0; i < YearArray.Length; i++)
            {
                YearArray[i] = (i + 2020).ToString();
            }
            return YearArray;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(monthlyReportPage, "Monthly Report Printing");
            }
        }
    }
}