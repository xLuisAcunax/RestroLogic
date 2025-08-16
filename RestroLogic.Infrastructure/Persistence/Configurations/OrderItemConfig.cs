using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestroLogic.Domain.Entities;

namespace RestroLogic.Infrastructure.Persistence.Configurations
{
    public sealed class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.UnitPrice).HasPrecision(18, 2);
            b.Property(x => x.LineTotal).HasPrecision(18, 2);
            // No configures aquí HasOne<Order>()... ya lo define OrderConfig
        }
    }
}
