using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Entities;

namespace RestroLogic.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(); 
            modelBuilder.Entity<OrderItem>();

            modelBuilder.Entity<Customer>(customer =>
            {
                customer.OwnsOne(c => c.Email, email =>
                {
                    email.Property(e => e.Value);
                });
            });

            modelBuilder.Entity<Product>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(120);
                b.Property(p => p.Description).HasMaxLength(500);
                b.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

        }
    }
}
