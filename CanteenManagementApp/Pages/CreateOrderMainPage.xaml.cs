using CanteenManagementApp.MVVM.ViewModel;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CanteenManagementApp.Pages
{
    /// <summary>
    /// Interaction logic for CreateOrderMainPage.xaml
    /// </summary>
    public partial class CreateOrderMainPage : Page
    {
        public CreateOrderViewModel CreateOrderVM { get; set; }

        public CreateOrderMainPage(CreateOrderViewModel viewModel)
        {
            InitializeComponent();
            CreateOrderVM = viewModel;
            DataContext = CreateOrderVM;
        }


        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SetCorrespondingLayout();
        }

        public void SetCorrespondingLayout()
        {
            if (CreateOrderViewModel.HasCustomer)
            {
                CustomerId.Content = CreateOrderViewModel.Customer.Id;
            }
            else
            {
                CustomerId.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Decrease_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            foodListView.SelectedItem = btn.DataContext;

            int indexSelected = foodListView.Items.IndexOf(btn.DataContext);
            if (indexSelected != -1)
            {
                if (CreateOrderVM.ListFoodItemOrder[indexSelected].Amount > 0)
                {
                    CreateOrderVM.ListFoodItemOrder[indexSelected].Amount--;
                }
            }
            CreateOrderVM.UpdateTotalOrder();

        }

        private void Increase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            foodListView.SelectedItem = btn.DataContext;

            int indexSelected = foodListView.Items.IndexOf(btn.DataContext);
            if (indexSelected != -1)
                CreateOrderVM.ListFoodItemOrder[indexSelected].Amount++;

            CreateOrderVM.UpdateTotalOrder();
        }

        private void DecreaseInventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            inventoryListView.SelectedItem = btn.DataContext;

            int indexSelected = inventoryListView.Items.IndexOf(btn.DataContext);
            if (indexSelected != -1)
            {
                if (CreateOrderVM.ListInventoryItemOrder[indexSelected].Amount > 0)
                    CreateOrderVM.ListInventoryItemOrder[indexSelected].Amount--;
            }
            CreateOrderVM.UpdateTotalOrder();
        }

        private void IncreaseInventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
                Button btn = (Button)e.OriginalSource;
                inventoryListView.SelectedItem = btn.DataContext;

                int indexSelected = inventoryListView.Items.IndexOf(btn.DataContext);
                if (indexSelected != -1)
                    CreateOrderVM.ListInventoryItemOrder[indexSelected].Amount++;

                CreateOrderVM.UpdateTotalOrder();
         }

        private void List_PreviewLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem clickedOnItem = (ListViewItem)GetParentDependencyObjectFromVisualTree((DependencyObject)e.MouseDevice.DirectlyOver, typeof(ListViewItem));

            if (clickedOnItem != null)
            {
                if (!clickedOnItem.IsSelected)
                {
                    clickedOnItem.IsSelected = true;
                    clickedOnItem.Focus();
                }
            }


        }

        private DependencyObject GetParentDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
                //Walk the visual tree to get the parent of this control
                DependencyObject parent = startObject;
                while (parent != null)
                {
                    if (type.IsInstanceOfType(parent))
                        break;
                    else
                        parent = VisualTreeHelper.GetParent(parent);
                }

                return parent;
        }
    }
}
