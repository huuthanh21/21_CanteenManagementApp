using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.MVVM.ViewModel;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
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

        private void SetCorrespondingLayout()
        {
            if (CreateOrderVM.HasCustomer)
            {
                CustomerId.Content = CreateOrderVM.Customer.Id;
            }
            else
            {
                CustomerId.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void ButtonContinue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM?.NavigatePaymentPageCommand.Execute(null);
        }

        private void ButtonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateOrderVM?.NavigateMainPageCommand.Execute(null);
        }

        private void Decrease_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            foodListView.SelectedItem = btn.DataContext;

            int indexSelected = foodListView.Items.IndexOf(btn.DataContext);
            if (indexSelected != -1)
            {
                if (CreateOrderVM._ListFoodItemOrder[indexSelected]._amount > 0)
                {
                    CreateOrderVM._ListFoodItemOrder[indexSelected]._amount--;
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
                CreateOrderVM._ListFoodItemOrder[indexSelected]._amount++;

            CreateOrderVM.UpdateTotalOrder();
        }

        private void DecreaseInventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            inventoryListView.SelectedItem = btn.DataContext;

            int indexSelected = inventoryListView.Items.IndexOf(btn.DataContext);
            if (indexSelected != -1)
            {
                if (CreateOrderVM._ListInventoryItemOrder[indexSelected]._amount > 0)
                    CreateOrderVM._ListInventoryItemOrder[indexSelected]._amount--;
            }
            CreateOrderVM.UpdateTotalOrder();
        }


        private void IncreaseInventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
                Button btn = (Button)e.OriginalSource;
                inventoryListView.SelectedItem = btn.DataContext;

                int indexSelected = inventoryListView.Items.IndexOf(btn.DataContext);
                if (indexSelected != -1)
                    CreateOrderVM._ListInventoryItemOrder[indexSelected]._amount++;

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
