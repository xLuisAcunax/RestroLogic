using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestroLogic.Domain.Entities;

namespace RestroLogic.Infrastructure.Persistence.Configurations
{
    public sealed class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Status).IsRequired().HasMaxLength(20);
            b.Property(x => x.Total).HasPrecision(18, 2);

            // Relación principal: Order -> OrderItem
            b.HasMany(o => o.Items)
             .WithOne()
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);

            // Si quieres forzar el backing field, hazlo SOBRE la navegación "Items"
            b.Metadata.FindNavigation(nameof(Order.Items))!
                      .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
