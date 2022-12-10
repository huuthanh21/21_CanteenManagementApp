using Microsoft.EntityFrameworkCore;

namespace CanteenManagementApp.MVVM.Model
{
    public class CanteenContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<Receipt_Item> Receipt_Items { get; set; }

        private const string connectionString = @"
                Server=.\SQLEXPRESS;
                Database=CANTEEN_DATABASE;
                Encrypt=False;
                Trusted_Connection=True;
                MultipleActiveResultSets=true;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt_Item>()
                .HasKey(ri => new { ri.ReceiptId, ri.ItemId });
            modelBuilder.Entity<Receipt>()
                 .HasMany(r => r.Receipt_Items)
                 .WithOne(ri => ri.Receipt);

            modelBuilder.Entity<Item>()
                 .HasMany(r => r.Receipt_Items)
                 .WithOne(ri => ri.Item);
        }
    }
}