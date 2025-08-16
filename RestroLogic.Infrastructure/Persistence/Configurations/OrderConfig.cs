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
            b.HasOne<Table>().WithMany().HasForeignKey(x => x.TableId).OnDelete(DeleteBehavior.Restrict);
            b.Navigation("_items").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
