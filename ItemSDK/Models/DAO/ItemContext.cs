using Ecremmoce.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItemSDK.Models
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemSDK.Models.ItemContext> options) : base(options)
        {

        }

        public DbSet<Item> Items{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .ToContainer("Items")
                .HasPartitionKey(nameof(Item.CompanyId))
                .HasKey(e => e.ItemId);

            modelBuilder.Entity<Item>()
                .Property(e => e.Source)
                .HasConversion(new EnumToStringConverter<EPlatform>());

            modelBuilder.Entity<Item>()
                .Property(e => e.CompanyId)
                .HasConversion(new GuidToStringConverter());
        }

    }
}
