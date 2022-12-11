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
            // Implementation on app's first run
            _ = ImplementFirstRun();
            // Write your testing code in this method
            _ = QueryTest();
        }

        private static async Task ImplementFirstRun()
        {
            if (DbQueries.CustomerQueries.GetCustomerById("-1") == null)
            {
                await DbQueries.CustomerQueries.InsertCustomerAsync("-1", "Không có tài khoản", "Trống");
            }
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

        public static async Task CreateDatabase()
        {
            Debug.WriteLine("Vao create ne");
            await DropDatabase();

            using var dbContext = new CanteenContext();
            String databasename = dbContext.Database.GetDbConnection().Database;
            Debug.WriteLine("Create ne");
            bool result = await dbContext.Database.EnsureCreatedAsync();
            Debug.WriteLine(result ? "created successfully" : "already created");
        }

        public static async Task DropDatabase()
        {
            Debug.WriteLine("Vao drop ne");
            using var context = new CanteenContext();
            String databasename = context.Database.GetDbConnection().Database;

            Debug.WriteLine("Drop ne");
            bool deleted = await context.Database.EnsureDeletedAsync();
            string deletionInfo = deleted ? "đã xóa" : "không xóa được";
            Debug.WriteLine($"{databasename} {deletionInfo}");
        }

        public static async Task QueryTest()
        {
            using var dbContext = new CanteenContext();

            var items = await DbQueries.CustomerQueries.GetFrequentlyBoughtItemsByCustomerIdAsync("20120582");

            foreach (var item in items)
            {
                Debug.WriteLine(item.Name);
            }
        }
    }
}