using InvestimentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestimentApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderCalculation> OrderCalculations { get; set; }
        public DbSet<OrderBookItem> OrderBookItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<Order>()
                .Property(p => p.Quantity)
                .HasColumnType("decimal(16,12)");        

            modelBuilder.Entity<Order>()
               .Property(p => p.Price)               
               .HasColumnType("decimal");

            modelBuilder.Entity<OrderCalculation>()
               .HasKey(o => o.OrderCalculationId);

            modelBuilder.Entity<OrderCalculation>()
               .Property(p => p.TotalQuantity)
               .HasColumnType("decimal(20,15)");

            modelBuilder.Entity<OrderCalculation>()
               .Property(p => p.TotalPrice)
               .HasColumnType("decimal(15,5)");

            modelBuilder.Entity<OrderBookItem>()
               .HasKey(obi => obi.OrderBookItemId);

            modelBuilder.Entity<OrderBookItem>()
               .Property(p => p.Quantity)
               .HasColumnType("decimal(16,12)");

            modelBuilder.Entity<OrderBookItem>()
               .Property(p => p.Price)
               .HasColumnType("decimal(10,3)");            
        }
    }
}
