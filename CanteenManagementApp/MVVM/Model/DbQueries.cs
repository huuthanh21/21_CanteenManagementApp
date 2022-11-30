using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenManagementApp.MVVM.Model
{
    public static class DbQueries
    {
        /* Query */
        public static Customer GetCustomer(string Id)
        {
            using var context = new CanteenContext();

            var customer = context.Customers
                                    .Where(c => c.Id == Id)
                                    .FirstOrDefault();
            return customer;
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
                Debug.WriteLine($"Save {rows} customers");
            }
        }
        public static async Task InsertReceiptItemAsync(int receiptId, int itemId)
        {
            using var context = new CanteenContext();

            await context.Receipt_Items.AddAsync(new Receipt_Item
            {
                ReceiptId = receiptId,
                ItemId = itemId
            });

            int rows = await context.SaveChangesAsync();
            Debug.WriteLine($"Save {rows} receipt_items");
        }

        /* Update */



        /* Delete */



    }
}
