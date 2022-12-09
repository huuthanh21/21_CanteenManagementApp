using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
            }
            /* Update */

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

            public static async Task InsertItemAsync(Item item)
            {
                using var context = new CanteenContext();

                await context.Items.AddAsync(item);

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"Saved {rows} items");
            }
            /* Update */
            public static void UpdateItem(Item item)
            {
                using var context = new CanteenContext();

                Item oldItem = GetItemById(item.Id);
                oldItem.Name = item.Name;
                oldItem.Price = item.Price;
                oldItem.Description = item.Description;
                oldItem.Amount = item.Amount;

                int rows = context.SaveChanges();
                Debug.WriteLine($"{rows} items updated");
            }
            public static void UpdateItem(int id,string name,float price,string description,int amount)
            {
                using var context = new CanteenContext();
                var oldItem = context.Items
                               .Where(i => i.Id == id)
                               .FirstOrDefault();
                oldItem.Id = id;
                oldItem.Name = name;
                oldItem.Price = price;
                oldItem.Description = description;
                oldItem.Amount = amount;
                int rows = context.SaveChanges();
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

            /* Insert */

            /* Update */

            /* Delete */
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
                                        .Where(r => r.Customer.Id == customerId);

                return (List<Receipt>)receipts;
            }

            // Returns: List of Tuple(Item, BoughtAmount)
            public static List<Tuple<Item, int>> GetReceiptDetailsByReceipt(Receipt receipt)
            {
                using var context = new CanteenContext();

                var item_amounts = context.Receipt_Items
                                            .Join(context.Items, ri => ri.ItemId, i => i.Id, (ri, i) => new { ri, i })
                                            .Where(rii => rii.ri.ReceiptId.Equals(receipt.Id))
                                            .Select(rii => new Tuple<Item, int>(rii.i, rii.ri.Amount))
                                            .ToList();
                return item_amounts;
            }
            /* Insert */
            public static async Task InsertReceiptAsync(string customerId, List<Tuple<Item, int>> item_tuples, string paymentMethod, float total)
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
                await context.Receipts.AddAsync(receipt);

                int rows = await context.SaveChangesAsync();
                Debug.WriteLine($"Saved {rows} receipts");

                int receiptId = receipt.Id;

                foreach (Tuple<Item, int> item_tuple in item_tuples)
                {
                    await ReceiptItemQueries.InsertReceiptItemAsync(receiptId, item_tuple.Item1.Id, item_tuple.Item2);
                }
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
