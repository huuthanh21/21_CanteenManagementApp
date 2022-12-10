using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.View;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class ReportViewModel : ObservableObject
    {
        public static RelayCommand DailyReportCommand { get; set; }
        public static RelayCommand MonthlyReportCommand { get; set; }
        public static RelayCommand ReportCommand { get; set; }
        public DailyReport CreateDailyReport { get; set; }
        public MonthlyReport CreateMonthlyReport { get; set; }
        public static Report CreateReport { get; set; }
        public static CustomerViewModel CustomerVM { get; set; }

        public object CurrentPage { get; set; }

        public ReportViewModel()
        {
            CreateDailyReport = new DailyReport();
            CreateMonthlyReport = new MonthlyReport();
            CreateReport = new Report();

            CurrentPage = CreateReport;

            DailyReportCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateDailyReport;
            });

            MonthlyReportCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateMonthlyReport;
            });

            ReportCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateReport;
            });
        }
    }
}