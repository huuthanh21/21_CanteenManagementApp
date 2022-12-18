using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public static class DbQueries
    {
        public static class CustomerQueries
        {
            /* Query */

            public static Customer GetCustomerById(string customerId)
            {
                using var context = new CanteenContext();

                var customer = context.Customers
                                        .Where(c => c.Id == customerId)
                                        .FirstOrDefault();
                return customer;
            }

            public static async Task<List<Item>> GetFrequentlyBoughtItemsByCustomerIdAsync(string customerId)
            {
                using var context = new CanteenContext();
                var items = await context.Customers
                                    .Where(c => c.Id == customerId)
                                    .Join(context.Receipts, c => c.Id, r => r.CustomerId, (c, r) => new { r.Id })
                                    .Join(context.Receipt_Items, cr => cr.Id, ri => ri.ReceiptId, (cr, ri) => new { ri.Item, ri.Amount })
                                    .GroupBy(crri => crri.Item)
                                    .Select(itemGroup => new
                                    {
                                        Item = itemGroup.Key,
                                        Sum = itemGroup.Sum(crri => crri.Amount)
                                    })
                                    .OrderByDescending(i => i.Sum)
                                    .Select(i => i.Item)
                                    .Take(5)
                                    .ToListAsync();
                return items;
            }

            /* Insert */

            public static async Task InsertCustomerAsync(string customerId, string customerName, string customerType)
            {
                using var context = new CanteenContext();

                await context.Customers.AddAsync(new Customer
                {
                    Id = customerId,
                    Name = customerName,
                    CustomerType = customerType,
                    Balance = 0
                });

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"Saved {rows} customers");
            }

            /* Update */

            public static async Task TopUpCustomerBalance(Customer customer, float amount)
            {
                using var context = new CanteenContext();

                customer.Balance += amount;

                context.Customers.Update(customer);

                int rows = await context.SaveChangesAsync();

                Debug.WriteLine($"{rows} rows in Customer updated");
            }

            public static async Task UpdateCustomerBalanceOnPurchase(Customer customer, float orderCost)
            {
                using var context = new CanteenContext();

                customer.Balance -= orderCost;

                context.Customers.Update(customer);

                int rows = await context.SaveChangesAsync();

                Debug.WriteLine($"{rows} rows in Customer updated");
            }

            /* Delete */
        }

        public static class ItemQueries
        {
            /* Query */

            public static List<Item> GetItemsByType(int itemType)
            {
                using var context = new CanteenContext();

                var items = context.Items
                                .Where(i => i.Type == itemType)
                                .ToList();

                if (itemType == 1)
                {
                    items.RemoveAll(item => item.Id == 100);
                }

                return items;
            }

            public static Item GetItemById(int itemId)
            {
                using var context = new CanteenContext();

                var item = context.Items
                                .Where(i => i.Id == itemId)
                                .FirstOrDefault();

                return item;
            }

            /* Insert */

            public static async Task InsertItemAsync(int itemType, string itemName, float itemPrice, string description = "", int amount = 0)
            {
                using var context = new CanteenContext();

                await context.Items.AddAsync(new Item
                {
                    Type = itemType,
                    Name = itemName,
                    Price = itemPrice,
                    Description = description,
                    Amount = amount
                });

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"Saved {rows} items");
            }

            public static async Task InsertItemAsync(Item item, bool identityInsert = false)
            {
                using var context = new CanteenContext();
                using var transaction = context.Database.BeginTransaction();
                await context.Items.AddAsync(item);

                int rows;
                if (identityInsert)
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Item ON;");
                    rows = await context.SaveChangesAsync();
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Item OFF;");
                    transaction.Commit();
                }
                else
                {
                    rows = await context.SaveChangesAsync();
                }
                Debug.WriteLine($"Saved {rows} items");
            }

            /* Update */

            public static async Task UpdateItem(Item item)
            {
                using var context = new CanteenContext();

                Item oldItem = GetItemById(item.Id);
                context.Items.Update(oldItem);
                if (oldItem is not null)
                {
                    oldItem.Name = item.Name;
                    oldItem.Price = item.Price;
                    oldItem.Description = item.Description;
                    oldItem.Amount = item.Amount;
                }

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"{rows} items updated");
            }

            public static void UpdateItem(int id, string name, float price, string description, int amount)
            {
                using var context = new CanteenContext();
                var oldItem = context.Items
                               .Where(i => i.Id == id)
                               .FirstOrDefault();

                if (oldItem is not null)
                {
                    oldItem.Id = id;
                    oldItem.Name = name;
                    oldItem.Price = price;
                    oldItem.Description = description;
                    oldItem.Amount = amount;
                }

                int rows = context.SaveChanges();
                Debug.WriteLine($"{rows} items updated");
            }

            public static async Task UpdateItemAmountOnPurchase(IEnumerable<ItemOrder> itemOrders)
            {
                using var context = new CanteenContext();

                foreach (var itemOrder in itemOrders)
                {
                    if (itemOrder.Item.Id == 100) continue;

                    itemOrder.Item.Amount -= itemOrder.Amount;
                    await UpdateItem(itemOrder.Item);
                }

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"{rows} items updated");
            }

            /* Delete */

            public static void DeleteItemById(int itemId)
            {
                using var context = new CanteenContext();

                Item item = GetItemById(itemId);
                context.Items.Remove(item);

                int rows = context.SaveChanges();
                Debug.WriteLine($"{rows} items deleted");
            }
        }

        public static class MenuQueries
        {
            /* Query */

            public static List<Item> GetYesterdayMenuItems()
            {
                using var context = new CanteenContext();

                DateTime yesterday = DateTime.Today.AddDays(-1);
                var items = context.Menus
                                    .Where(m => m.Date.Date == yesterday.Date)
                                    .Select(m => m.Item)
                                    .ToList();
                return items;
            }

            public static List<Item> GetTodayMenuItems()
            {
                using var context = new CanteenContext();

                var items = context.Menus
                                    .Where(m => m.Date.Date == DateTime.Today.Date)
                                    .Select(m => m.Item)
                                    .ToList();
                return items;
            }

            /* Insert */

            public static async Task InsertMenuItem(Item item)
            {
                using var context = new CanteenContext();

                DateTime today = DateTime.Today.Date;

                int rowsAffected = await context.Database.
                    ExecuteSqlRawAsync("IF NOT EXISTS (SELECT * FROM MENU" +
                                        $" WHERE Date='{today:yyyy-MM-dd}' AND ItemId={item.Id})" +
                                        " BEGIN " +
                                            " INSERT INTO MENU" +
                                            $" VALUES ('{today:yyyy-MM-dd}', {item.Id}) " +
                                        " END");
                if (item.Amount != 0)
                {
                    await UpdateAmountMenuItem(item, item.Amount);
                }

                Debug.WriteLine($"{rowsAffected} inserted in Menu");
            }

            public static async Task InsertMenuItems(params Item[] items)
            {
                using var context = new CanteenContext();

                DateTime today = DateTime.Today.Date;

                int rowsAffected = 0;
                foreach (var id in items.Select(i => i.Id))
                {
                    rowsAffected += await context.Database.
                        ExecuteSqlRawAsync("IF NOT EXISTS (SELECT * FROM MENU" +
                                            $" WHERE Date='{today:yyyy-MM-dd}' AND ItemId={id})" +
                                            " BEGIN " +
                                                " INSERT INTO MENU" +
                                                $" VALUES ('{today:yyyy-MM-dd}', {id}) " +
                                            " END");
                }

                Debug.WriteLine($"{rowsAffected} inserted in Menu");
            }

            public static async Task InsertMenuItems(IEnumerable<Item> items)
            {
                using var context = new CanteenContext();

                DateTime today = DateTime.Today.Date;

                int rowsAffected = 0;
                foreach (var id in items.Select(i => i.Id))
                {
                    rowsAffected += await context.Database.
                        ExecuteSqlRawAsync("IF NOT EXISTS (SELECT * FROM MENU" +
                                            $" WHERE Date='{today:yyyy-MM-dd}' AND ItemId={id})" +
                                            " BEGIN " +
                                                " INSERT INTO MENU" +
                                                $" VALUES ('{today:yyyy-MM-dd}', {id}) " +
                                            " END");
                }

                Debug.WriteLine($"{rowsAffected} inserted in Menu");
            }

            /* Update */

            public static async Task UpdateAmountMenuItem(Item item, int amount)
            {
                using var context = new CanteenContext();
                item.Amount = amount;
                context.Items.Update(item);

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"{rows} updated in items");
            }

            public static async Task ResetAmountMenuItems(IEnumerable<Item> items)
            {
                using var context = new CanteenContext();
                foreach (var item in items)
                {
                    item.Amount = 0;
                }
                context.Items.UpdateRange(items);

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"{rows} updated in items");
            }

            public static async Task ResetAmountMenuItems(params Item[] items)
            {
                using var context = new CanteenContext();
                foreach (var item in items)
                {
                    item.Amount = 0;
                }
                context.Items.UpdateRange(items);

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"{rows} updated in items");
            }

            /* Delete */

            public static void DeleteMenuItem(Item item)
            {
                using var context = new CanteenContext();

                var today = DateTime.Today.Date;

                int rows = context.Database.ExecuteSqlRaw("DELETE FROM Menu" +
                                                $" WHERE Date='{today:yyyy-MM-dd}' AND ItemId={item.Id}");

                Debug.WriteLine($"{rows} deleted in menu");
            }
        }

        public static class ReceiptQueries
        {
            /* Query */

            public static Receipt GetReceiptById(int receiptId)
            {
                using var context = new CanteenContext();

                var receipt = context.Receipts
                                        .Where(r => r.Id == receiptId)
                                        .FirstOrDefault();

                return receipt;
            }

            public static List<Receipt> GetReceiptsByCustomerId(string customerId)
            {
                using var context = new CanteenContext();

                var receipts = context.Receipts
                                        .Where(r => r.Customer.Id == customerId)
                                        .ToList();

                return receipts;
            }

            public static List<ItemOrder> GetReceiptDetailsByReceipt(Receipt receipt)
            {
                using var context = new CanteenContext();

                var itemOrders = context.Receipt_Items
                                            .Join(context.Items, ri => ri.ItemId, i => i.Id, (ri, i) => new { ri, i })
                                            .Where(rii => rii.ri.ReceiptId.Equals(receipt.Id))
                                            .Select(rii => new ItemOrder { Item = rii.i, Amount = rii.ri.Amount })
                                            .ToList();
                return itemOrders;
            }

            public static async Task<float> GetDayRevenueAsync(DateOnly date)
            {
                using var context = new CanteenContext();

                float revenue = await context.Receipts
                                    .Where(r => r.DateTime.Day == date.Day && r.DateTime.Month == date.Month && r.DateTime.Year == date.Year)
                                    .SumAsync(r => r.Total);

                return revenue;
            }

            // Returns 2 records with the structure of (Type, SalesAmount, Revenue)
            public static async Task<Tuple<int, float>> GetItemSalesByDayAsync(DateOnly date, int itemType)
            {
                using var context = new CanteenContext();

                var sales = await context.Receipts
                                        .Where(r => r.DateTime.Day == date.Day && r.DateTime.Month == date.Month && r.DateTime.Year == date.Year)
                                        .Join(context.Receipt_Items, r => r.Id, ri => ri.ReceiptId, (r, ri) => new { ri.ItemId, ri.Amount })
                                        .Join(context.Items, ri => ri.ItemId, i => i.Id, (ri, i) => new { i.Type, ri.Amount, i.Price })
                                        .Where(rii => rii.Type == itemType)
                                        .GroupBy(i => i.Type)
                                        .Select(itemGroup => new
                                        {
                                            Type = itemGroup.Key,
                                            SalesAmount = itemGroup.Sum(i => i.Amount),
                                            Revenue = itemGroup.Sum(i => i.Price * i.Amount)
                                        })
                                        .Select(i => new Tuple<int, float>(
                                                i.SalesAmount,
                                                i.Revenue
                                            ))
                                        .FirstOrDefaultAsync();
                if (sales is null)
                {
                    return new Tuple<int, float>(0, 0);
                }

                return sales;
            }

            /* Insert */

            public static async Task<int> InsertReceiptAsync(string customerId, List<ItemOrder> item_orders, string paymentMethod, float total)
            {
                using var context = new CanteenContext();

                var datetime = DateTime.Now;

                Receipt receipt = new()
                {
                    CustomerId = customerId,
                    PaymentMethod = paymentMethod,
                    DateTime = datetime,
                    Total = total
                };
                context.Receipts.Add(receipt);

                int rows = context.SaveChanges();
                Debug.WriteLine($"Saved {rows} receipts");

                int receiptId = receipt.Id;

                foreach (ItemOrder item_order in item_orders)
                {
                    await ReceiptItemQueries.InsertReceiptItemAsync(receiptId, item_order.Item.Id, item_order.Amount);
                }
                return receiptId;
            }

            /* Update */

            /* Delete */
        }

        public static class ReceiptItemQueries
        {
            /* Query */

            /* Insert */

            public static async Task InsertReceiptItemAsync(int receiptId, int itemId, int amount)
            {
                using var context = new CanteenContext();

                await context.Receipt_Items.AddAsync(new Receipt_Item
                {
                    ReceiptId = receiptId,
                    ItemId = itemId,
                    Amount = amount
                });

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"Saved {rows} receipt_items");
            }

            /* Update */

            /* Delete */
        }
    }
}