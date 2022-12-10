﻿using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CreateOrderViewModel : ObservableObject
    {
        public RelayCommand NavigateMainPageCommand { get; set; }
        public RelayCommand NavigateMainPageWithResetCommand { get; set; }
        public RelayCommand NavigatePaymentPageCommand { get; set; }
        public ICommand NavigateReceiptPageCommand { get; set; }

        public object CurrentPage { get; set; }

        public static Customer Customer { get; set; } = null;

        public static bool HasCustomer
        { get => Customer != null; }

        public bool PayInCash { get; set; } = true;
        public RelayCommand TogglePayInCashCommand { get; set; }
        public RelayCommand TogglePayThroughAccountCommand { get; set; }
        public int ReceiptId { get; set; }

        public CreateOrderMainPage CreateOrderMainPage { get; set; }
        public CreateOrderPaymentPage CreateOrderPaymentPage { get; set; }
        public CreateOrderReceiptPage CreateOrderReceiptPage { get; set; }

        public ObservableCollection<ItemOrder> ListFoodItemOrder { get; set; }
        public ObservableCollection<ItemOrder> ListInventoryItemOrder { get; set; }

        // total items selected by customer
        public ObservableCollection<ItemOrder> TotalItemOrder { get; set; }

        public float TotalOrderCost
        {
            get
            {
                return CalculateOrderCost();
            }
        }

        public float GivenMoney { get; set; } = 0;

        public float ChangeOfReceipt
        {
            get { return GivenMoney - TotalOrderCost; }
        }

        private readonly CollectionViewSource _inventoryItemsCollection;
        private readonly CollectionViewSource _foodItemsCollection;
        private readonly CollectionViewSource _totalOrderItemsCollection;

        public ICollectionView FoodSourceCollection => _foodItemsCollection.View;
        public ICollectionView InventorySourceCollection => _inventoryItemsCollection.View;
        public ICollectionView TotalOrderSourceCollection => _totalOrderItemsCollection.View;

        public ICommand IncreaseAmountOrderCommand { get; set; }

        public ICommand RemoveItemOrderCommand { get; set; }

        //----------------
        public CreateOrderViewModel()
        {
            CreateOrderMainPage = new CreateOrderMainPage(this);
            CreateOrderPaymentPage = new CreateOrderPaymentPage(this);
            CreateOrderReceiptPage = new CreateOrderReceiptPage(this);

            CurrentPage = CreateOrderMainPage;

            NavigateMainPageCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderMainPage;
                NotifyPropertyChanged(nameof(TotalOrderCost));
            });

            NavigateMainPageWithResetCommand = new RelayCommand(o =>
            {
                CurrentPage = CreateOrderMainPage;
                TotalItemOrder.Clear();
                Customer = null;
                CreateOrderMainPage.SetCorrespondingLayout();
                NotifyPropertyChanged(nameof(TotalOrderCost));
                NotifyPropertyChanged(nameof(TotalItemOrder));
                foreach (ItemOrder itemOrder in ListFoodItemOrder)
                {
                    itemOrder.Amount = 0;
                }
                foreach (ItemOrder itemOrder in ListInventoryItemOrder)
                {
                    itemOrder.Amount = 0;
                }
            });

            NavigatePaymentPageCommand = new RelayCommand(o =>
            {
                if (TotalItemOrder.Count != 0)
                    CurrentPage = CreateOrderPaymentPage;
                else
                    MessageBox.Show("Bạn chưa chọn mặt hàng nào.", "Không có mặt hàng", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            NavigateReceiptPageCommand = new RelayCommand<TextBox>(givenMoneyTextbox => true, async givenMoneyTextbox =>
            {
                if (TotalItemOrder.Count != 0)
                {
                    if (PayInCash)
                    {
                        // Get the TextboxInput inside the template
                        var template = givenMoneyTextbox.Template;
                        var textbox = (TextBox)template.FindName("TextboxInput", givenMoneyTextbox);
                        if (!string.IsNullOrEmpty(textbox.Text) && textbox.Text.All(c => c >= '0' && c <= '9'))
                        {
                            if (float.Parse(textbox.Text) >= TotalOrderCost)
                            {
                                CurrentPage = CreateOrderReceiptPage;
                                GivenMoney = float.Parse(textbox.Text);
                                ReceiptId = await DbQueries.ReceiptQueries.InsertReceiptAsync(HasCustomer ? Customer.Id : "-1", TotalItemOrder.ToList(), PayInCash ? "Tiền mặt" : "Trả trước", TotalOrderCost);
                            }
                            else
                            {
                                MessageBox.Show("Số tiền nhận phải lớn hơn giá trị của đơn hàng.", "Không đủ tiền", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            }
                        }
                    }
                    else
                    {
                        if (Customer.Balance < TotalOrderCost)
                        {
                            MessageBox.Show("Khách hàng không đủ tiền trong tài khoản.", "Không đủ tiền", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        }
                        else
                        {
                            ReceiptId = await DbQueries.ReceiptQueries.InsertReceiptAsync(HasCustomer ? Customer.Id : "-1", TotalItemOrder.ToList(), PayInCash ? "Tiền mặt" : "Trả trước", TotalOrderCost);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bạn đã xóa hết mặt hàng, vui lòng trở về để chọn.", "Không có mặt hàng", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });

            TogglePayInCashCommand = new RelayCommand(o => PayInCash = true);
            TogglePayThroughAccountCommand = new RelayCommand(o => PayInCash = false);

            // Set up data to display
            ListFoodItemOrder = new ObservableCollection<ItemOrder> { };
            ListInventoryItemOrder = new ObservableCollection<ItemOrder> { };
            FillItemOrderLists();

            TotalItemOrder = new ObservableCollection<ItemOrder> { };

            _foodItemsCollection = new CollectionViewSource { Source = ListFoodItemOrder };
            _totalOrderItemsCollection = new CollectionViewSource { Source = TotalItemOrder };
            _inventoryItemsCollection = new CollectionViewSource { Source = ListInventoryItemOrder };
            IncreaseAmountOrderCommand = new RelayCommand<CreateOrderMainPage>((parameter) => true, (parameter) => IncreaseAmountOrder(parameter));

            RemoveItemOrderCommand = new RelayCommand<int>(itemId => true, itemId =>
            {
                foreach (ItemOrder itemOrder in TotalItemOrder)
                {
                    if (itemOrder.Item.Id == itemId)
                    {
                        TotalItemOrder.Remove(itemOrder);
                        itemOrder.Amount = 0;
                        NotifyPropertyChanged(nameof(TotalOrderCost));
                        break;
                    }
                }
            });
        }

        private void FillItemOrderLists()
        {
            foreach (Item item in new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(0)))
            {
                ListFoodItemOrder.Add(new ItemOrder() { Item = (Item)item.Clone() });
            }
            foreach (Item item in new ObservableCollection<Item>(DbQueries.ItemQueries.GetItemsByType(1)))
            {
                ListInventoryItemOrder.Add(new ItemOrder() { Item = (Item)item.Clone() });
            }
        }

        private void IncreaseAmountOrder(CreateOrderMainPage parameter)
        {
            int indexSelected = parameter.foodListView.SelectedIndex;
            ListFoodItemOrder[indexSelected].Amount++;
        }

        private float CalculateOrderCost()
        {
            float totalCost = 0;

            foreach (ItemOrder itemOrder in TotalItemOrder)
            {
                totalCost += (itemOrder.Item.Price * itemOrder.Amount);
            }
            return totalCost;
        }

        public void UpdateTotalOrder()
        {
            foreach (ItemOrder itemOrder in ListFoodItemOrder)
            {
                if (!TotalItemOrder.Contains(itemOrder))
                {
                    if (itemOrder.Amount > 0)
                    {
                        TotalItemOrder.Add(itemOrder);
                    }
                }
                else
                {
                    if (itemOrder.Amount <= 0)
                    {
                        TotalItemOrder.Remove(itemOrder);
                    }
                }
            }
            foreach (ItemOrder itemOrder in ListInventoryItemOrder)
            {
                if (!TotalItemOrder.Contains(itemOrder))
                {
                    if (itemOrder.Amount > 0)
                    {
                        TotalItemOrder.Add(itemOrder);
                    }
                }
                else
                {
                    if (itemOrder.Amount <= 0)
                    {
                        TotalItemOrder.Remove(itemOrder);
                    }
                }
            }
            NotifyPropertyChanged(nameof(TotalOrderCost));
        }
    }
}