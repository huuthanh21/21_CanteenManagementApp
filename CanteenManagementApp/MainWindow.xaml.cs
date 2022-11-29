using System.Threading.Tasks;
using System.Windows;
using System;
using System.Windows.Input;
using CanteenManagementApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CanteenManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CreateDatabase();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ControlButtonMinumize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ControlButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static async void CreateDatabase()
        {
            await DropDatabase();
            using var dbContext = new CanteenContext();
            bool result = await dbContext.Database.EnsureCreatedAsync();
            _ = result ? "created successfully" : "already created";
        }

        public static async Task DropDatabase()
        {

            using var context = new CanteenContext();
            String databasename = context.Database.GetDbConnection().Database;

            bool deleted = await context.Database.EnsureDeletedAsync();
            string deletionInfo = deleted ? "đã xóa" : "không xóa được";
            Debug.WriteLine($"{databasename} {deletionInfo}");

        }

    }
}
