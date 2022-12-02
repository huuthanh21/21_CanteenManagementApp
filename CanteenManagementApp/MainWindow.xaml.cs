using System.Threading.Tasks;
using System.Windows;
using System;
using System.Windows.Input;
using CanteenManagementApp.MVVM.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Collections.Generic;

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
            Debug.WriteLine("Vao create ne");
            /*await DropDatabase();*/
            using var dbContext = new CanteenContext();
            Debug.WriteLine("Create ne");
            String databasename = dbContext.Database.GetDbConnection().Database;
            bool result = await dbContext.Database.EnsureCreatedAsync();
            Debug.WriteLine(result ? "created successfully" : "already created");

            /*var item1 = new Item { Type = 0, Name = "Phở", Price = 35000, Description = "Ngon vãi cả chưởng" };
            var item2 = new Item { Type = 0, Name = "Bún bò", Price = 35000, Description = "Ngon vãi cả chưởng" };
            var item3 = new Item { Type = 1, Name = "Cocacola", Price = 10000, Description = "Tệ" };
            await DbQueries.ItemQueries.InsertItemAsync(item1);
            await DbQueries.ItemQueries.InsertItemAsync(item2);
            await DbQueries.ItemQueries.InsertItemAsync(item3);*/

         /*   var tuple1 = new Tuple<Item, int>(item1, 2);
            var tuple2 = new Tuple<Item, int>(item2, 1);
            var tuple3 = new Tuple<Item, int>(item3, 3);

            var tuples = new List<Tuple<Item, int>>
            {
                tuple1,
                tuple2,
                tuple3
            };*/

           /* await DbQueries.CustomerQueries.InsertCustomerAsync("20120582", "Trần Hữu Thành", "Sinh viên");*/

            /*await DbQueries.ReceiptQueries.InsertReceiptAsync("20120582", tuples, "Tiền mặt", 100000);*/
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

    }
}
