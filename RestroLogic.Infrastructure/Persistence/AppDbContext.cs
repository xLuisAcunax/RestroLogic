using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Sales;

namespace RestroLogic.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(o => o.Id);
                e.Property(o => o.TableNumber).IsRequired();
                e.OwnsMany(o => o.Items, items =>
                {
                    items.WithOwner().HasForeignKey(i => i.OrderId);
                    items.Property(i => i.MenuItemName).HasMaxLength(200);
                    items.OwnsOne(i => i.UnitPrice, m =>
                    {
                        m.Property(p => p.Amount).HasColumnType("decimal(18,2)");
                        m.Property(p => p.Currency).HasMaxLength(3);
                    });
                    items.OwnsOne(i => i.Quantity, q =>
                    {
                        q.Property(p => p.Value).HasColumnName("Quantity");
                    });
                });
            });

            modelBuilder.Entity<Order>().Navigation(nameof(Order.Items))
                .UsePropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
