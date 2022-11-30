using System.Windows;
using System.Windows.Controls;

namespace CanteenManagementApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Page
    {
        public Report()
        {
            InitializeComponent();
        }

        private void DailyReport_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ReportViewModel.DailyReportCommand.Execute(sender);
        }
        private void MonthlyReport_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ReportViewModel.MonthlyReportCommand.Execute(sender);
        }
    }
}
