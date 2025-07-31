using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Entities;

namespace RestroLogic.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().OwnsOne(p => p.Price);
            modelBuilder.Entity<Order>().OwnsMany(o => o.Items, items =>
            {
                modelBuilder.Entity<OrderItem>().OwnsOne(i => i.UnitPrice);
            });
        }
    }
}
