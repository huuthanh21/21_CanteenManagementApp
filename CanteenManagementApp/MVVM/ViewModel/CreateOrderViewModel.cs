using CanteenManagementApp.Core;
using CanteenManagementApp.MVVM.Model;
using CanteenManagementApp.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using CanteenManagementApp.MVVM.View;
using System.Threading.Tasks;
using System;
using PropertyChanged;

namespace CanteenManagementApp.MVVM.ViewModel
{
    public class CreateOrderViewModel : ObservableObject
    {
        private const int TOP_UP_ITEM_ID = 100;
        private object _textSearchBar;

        private readonly EventHandler TextSearchBarChanged;

        public object TextSearchBar
        {
            get { return _textSearchBar; }
            set
            {
                _textSearchBar = value;
                OnTextSearchBarChanged(EventArgs.Empty);
            }
        }

        public RelayCommand NavigateMainPageCommand { get; set; }
        public RelayCommand NavigateMainPageWithResetCommand { get; set; }
        public RelayCommand NavigatePaymentPageCommand { get; set; }
        public ICommand NavigateReceiptPageCommand { get; set; }

        public object CurrentPage { get; set; }

        public static Customer Customer { get; set; } = null;

        public static bool HasCustomer => Customer != null;

        public bool PayInCash { get; set; } = true;

        public static int TopUpAmount { get; set; } = 0;
        public bool NoTopUp => FindTopUpItemOrder() == null;

        public RelayCommand TogglePayInCashCommand { get; set; }
        public RelayCommand TogglePayThroughAccountCommand { get; set; }
        public RelayCommand TopUpCommand { get; set; }
        public ICommand ConfirmTopUpCommand { get; set; }
        public int ReceiptId { get; set; }

        public CreateOrderMainPage CreateOrderMainPage { get; set; }
        public CreateOrderPaymentPage CreateOrderPaymentPage { get; set; }
        public CreateOrderReceiptPage CreateOrderReceiptPage { get; set; }

        public ObservableCollection<ItemOrder> ListFoodItemOrder { get; set; }
        public ObservableCollection<ItemOrder> ListInventoryItemOrder { get; set; }

        // total items selected by customer
        public ObservableCollection<ItemOrder> TotalItemOrder { get; set; }

        public float TotalOrderCost => CalculateOrderCost();

        public float GivenMoney { get; set; } = 0;

        public float ChangeOfReceipt => GivenMoney - TotalOrderCost >= 0 ? GivenMoney - TotalOrderCost : 0;

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

                // Clear chosen amount in ItemOrder lists
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
                {
                    CurrentPage = CreateOrderPaymentPage;
                }
                else
                {
                    MessageBox.Show("Bạn chưa chọn mặt hàng nào.", "Không có mặt hàng", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });

            NavigateReceiptPageCommand = new RelayCommand<TextBox>(givenMoneyTextbox => true, async givenMoneyTextbox =>
            {
                if (TotalItemOrder.Count != 0)
                {
                    if (PayInCash)
                    {
                        await HandlePaymentInCash(givenMoneyTextbox);
                    }
                    else
                    {
                        await HandlePaymentThroughAccount();
                    }

                    // Update storage database
                    await DbQueries.ItemQueries.UpdateItemAmountOnPurchase(TotalItemOrder);
                }
                else
                {
                    MessageBox.Show("Bạn đã xóa hết mặt hàng, vui lòng trở về để chọn.", "Không có mặt hàng", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            });

            TogglePayInCashCommand = new RelayCommand(o => PayInCash = true);

            TogglePayThroughAccountCommand = new RelayCommand(o =>
            {
                // Pay through account is only allowed when there is no top-up item
                if (FindTopUpItemOrder() == null)
                {
                    PayInCash = false;
                }
            });

            TopUpCommand = new RelayCommand(o =>
            {
                var screen = new TopUpDialog(this);
                screen.ShowDialog();
                if (screen.DialogResult == true)
                {
                    screen.Close();
                }
            });

            ConfirmTopUpCommand = new RelayCommand<TextBox>(tb => true, tb =>
            {
                var template = tb.Template;
                var textbox = (TextBox)template.FindName("TextboxInput", tb);

                int topUpAmount = int.Parse(textbox.Text);
                if (topUpAmount % 10000 == 0)
                {
                    RegisterValidTopUp(topUpAmount);
                }
            });

            // Set up data to display
            ListFoodItemOrder = new ObservableCollection<ItemOrder> { };
            ListInventoryItemOrder = new ObservableCollection<ItemOrder> { };
            FillItemOrderLists();

            TotalItemOrder = new ObservableCollection<ItemOrder> { };
            if (HasCustomer && TopUpAmount != 0)
            {
                AddTopUpItem(TopUpAmount);
            }

            _foodItemsCollection = new CollectionViewSource { Source = ListFoodItemOrder };
            FoodSourceCollection.Filter = ItemFilter;
            _inventoryItemsCollection = new CollectionViewSource { Source = ListInventoryItemOrder };
            InventorySourceCollection.Filter = ItemFilter;
            _totalOrderItemsCollection = new CollectionViewSource { Source = TotalItemOrder };
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
                        NotifyPropertyChanged(nameof(NoTopUp));
                        break;
                    }
                }
            });

            TextSearchBarChanged += RefreshItemViewSource;
        }

        public bool ItemFilter(object itemOrder)
        {
            bool exists = (itemOrder as ItemOrder).Item.Amount > 0;
            if (string.IsNullOrEmpty(TextSearchBar as string))
            {
                return exists;
            }
            return (itemOrder as ItemOrder).Item.Name.Contains(TextSearchBar as string, StringComparison.OrdinalIgnoreCase) && exists;
        }

        private void RefreshItemViewSource(object sender, EventArgs e)
        {
            CollectionViewSource.GetDefaultView(FoodSourceCollection).Refresh();
            CollectionViewSource.GetDefaultView(InventorySourceCollection).Refresh();
        }

        [SuppressPropertyChangedWarnings]
        public void OnTextSearchBarChanged(EventArgs e)
        {
            TextSearchBarChanged?.Invoke(this, e);
        }

        public void AddTopUpItem(int amount)
        {
            TotalItemOrder.Add(new ItemOrder
            {
                Item = DbQueries.ItemQueries.GetItemById(TOP_UP_ITEM_ID),
                Amount = amount
            });
            NotifyPropertyChanged(nameof(TotalItemOrder));
            NotifyPropertyChanged(nameof(TotalOrderCost));
            NotifyPropertyChanged(nameof(NoTopUp));
        }

        private async Task HandlePaymentInCash(TextBox givenMoneyTextbox)
        {
            // Get the TextboxInput inside the template
            var template = givenMoneyTextbox.Template;
            var textbox = (TextBox)template.FindName("TextboxInput", givenMoneyTextbox);

            // Check if input is in correct format
            if (!string.IsNullOrEmpty(textbox.Text) && textbox.Text.All(c => c >= '0' && c <= '9'))
            {
                if (float.Parse(textbox.Text) >= TotalOrderCost)
                {
                    CurrentPage = CreateOrderReceiptPage;
                    GivenMoney = float.Parse(textbox.Text);
                    ReceiptId = await DbQueries.ReceiptQueries.InsertReceiptAsync(HasCustomer ? Customer.Id : "-1", TotalItemOrder.ToList(), PayInCash ? "Tiền mặt" : "Trả trước", TotalOrderCost);
                    await HandleTopUp();
                }
                else
                {
                    MessageBox.Show("Số tiền nhận phải lớn hơn giá trị của đơn hàng.", "Không đủ tiền", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
        }

        private async Task HandlePaymentThroughAccount()
        {
            if (Customer.Balance < TotalOrderCost)
            {
                MessageBox.Show("Khách hàng không đủ tiền trong tài khoản.", "Không đủ tiền", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                ReceiptId = await DbQueries.ReceiptQueries.InsertReceiptAsync(HasCustomer ? Customer.Id : "-1", TotalItemOrder.ToList(), PayInCash ? "Tiền mặt" : "Trả trước", TotalOrderCost);
                await DbQueries.CustomerQueries.UpdateCustomerBalanceOnPurchase(Customer, TotalOrderCost);
                CurrentPage = CreateOrderReceiptPage;
            }
        }

        private async Task HandleTopUp()
        {
            var topUpItem = FindTopUpItemOrder();
            if (topUpItem != null)
            {
                float topUpAmount = topUpItem.Amount * topUpItem.Item.Price;
                await DbQueries.CustomerQueries.TopUpCustomerBalance(Customer, topUpAmount);
            }
        }

        private void RegisterValidTopUp(int topUpAmount)
        {
            int amount = topUpAmount / 10000;

            var topUpItem = FindTopUpItemOrder();
            if (topUpItem != null)
            {
                topUpItem.Amount = amount;
            }
            else
            {
                TotalItemOrder.Add(new ItemOrder()
                {
                    Item = DbQueries.ItemQueries.GetItemById(TOP_UP_ITEM_ID),
                    Amount = amount
                });
            }

            NotifyPropertyChanged(nameof(TotalItemOrder));
            NotifyPropertyChanged(nameof(TotalOrderCost));
            NotifyPropertyChanged(nameof(NoTopUp));
        }

        // Returns the top-up ItemOrder
        private ItemOrder FindTopUpItemOrder()
        {
            foreach (ItemOrder itemOrder in TotalItemOrder)
            {
                if (itemOrder.Item.Id == TOP_UP_ITEM_ID)
                {
                    return itemOrder;
                }
            }
            return null;
        }

        private void FillItemOrderLists()
        {
            foreach (Item item in new ObservableCollection<Item>(DbQueries.MenuQueries.GetTodayMenuItems()))
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