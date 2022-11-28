﻿using System;
using System.Collections.Generic;
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

namespace CanteenManagementApp.MVVM.View
{

    public partial class DailyReport : Page
    {
        public DailyReport()
        {
            InitializeComponent();
        }
        private void ButtonLink_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ReportViewModel.ReportCommand.Execute(sender);
        }
    }
}