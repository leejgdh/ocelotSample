using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockSDK.Models
{
    public class StockContext : DbContext
    {

        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<StockHistory> StockHistories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StockHistory>()
                .HasOne(e => e.Stock)
                .WithMany(e => e.StockHistories)
                .HasForeignKey(e => e.StockId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
