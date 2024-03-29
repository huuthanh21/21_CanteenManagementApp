﻿using CanteenManagementApp.MVVM.ViewModel;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CanteenManagementApp.MVVM.Model;

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
                CustomerId.Visibility = Visibility.Collapsed;
                ButtonTopUp.Visibility = Visibility.Collapsed;
            }
        }

        private void Decrease_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            foodListView.SelectedItem = btn.DataContext;

            int indexSelected = CreateOrderVM.ListFoodItemOrder.IndexOf(foodListView.SelectedItem as ItemOrder);
            if (indexSelected != -1 && CreateOrderVM.ListFoodItemOrder[indexSelected].Amount > 0)
            {
                CreateOrderVM.ListFoodItemOrder[indexSelected].Amount--;
                CreateOrderVM.UpdateTotalOrder();
            }
        }

        private void Increase_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            foodListView.SelectedItem = btn.DataContext;

            int indexSelected = CreateOrderVM.ListFoodItemOrder.IndexOf(foodListView.SelectedItem as ItemOrder);
            if (indexSelected != -1)
            {
                ItemOrder selectedItem = CreateOrderVM.ListFoodItemOrder[indexSelected];
                if (selectedItem.Amount + 1 <= selectedItem.Item.Amount)
                {
                    selectedItem.Amount++;
                    CreateOrderVM.UpdateTotalOrder();
                }
                else
                {
                    MessageBox.Show("Đã vượt quá số lượng mặt hàng có sẵn", "Vượt số lượng", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void DecreaseInventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            inventoryListView.SelectedItem = btn.DataContext;

            int indexSelected = CreateOrderVM.ListInventoryItemOrder.IndexOf(inventoryListView.SelectedItem as ItemOrder);
            if (indexSelected != -1 && CreateOrderVM.ListInventoryItemOrder[indexSelected].Amount > 0)
            {
                CreateOrderVM.ListInventoryItemOrder[indexSelected].Amount--;
                CreateOrderVM.UpdateTotalOrder();
            }
        }

        private void IncreaseInventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = (Button)e.OriginalSource;
            inventoryListView.SelectedItem = btn.DataContext;
            ItemOrder itemOrder = inventoryListView.SelectedItem as ItemOrder;

            int indexSelected = CreateOrderVM.ListInventoryItemOrder.IndexOf(inventoryListView.SelectedItem as ItemOrder);
            if (indexSelected != -1 && itemOrder.Amount < itemOrder.Item.Amount)
            {
                itemOrder.Amount++;
                CreateOrderVM.UpdateTotalOrder();
            }
            else
            {
                MessageBox.Show("Vượt quá số lượng trong kho.", "Vượt quá số lượng", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void List_PreviewLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem clickedOnItem = (ListViewItem)GetParentDependencyObjectFromVisualTree((DependencyObject)e.MouseDevice.DirectlyOver, typeof(ListViewItem));

            if (clickedOnItem != null)
            {
                if (clickedOnItem.IsSelected)
                {
                    return;
                }
                clickedOnItem.IsSelected = true;
                clickedOnItem.Focus();
            }
        }

        private static DependencyObject GetParentDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
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