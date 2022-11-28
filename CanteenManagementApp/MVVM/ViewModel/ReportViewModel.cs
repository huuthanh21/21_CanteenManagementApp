﻿using CanteenManagementApp.Core;
using System;
using System.Windows.Input;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class ReportViewModel : ObservableObject
    {
        public static RelayCommand DailyReportCommand { get; set; }
        public static RelayCommand ReportCommand { get; set; }
        public View.DailyReport CreateDailyReport { get; set; }
        public static View.Report CreateReport { get; set; }
        public static CustomerViewModel CustomerVM { get; set; }
        private object _currentPage;
        public object CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; }
        }



        public ReportViewModel()
        {
            CreateDailyReport = new View.DailyReport();
            CreateReport = new View.Report();

            CurrentPage = CreateReport;

            DailyReportCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateDailyReport;
            });

            ReportCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateReport;
            });
        }
    }
}
