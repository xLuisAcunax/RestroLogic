using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestroLogic.Domain.Entities;

namespace RestroLogic.Infrastructure.Persistence.Configurations
{
    public sealed class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(150);
            b.Property(x => x.Price).HasPrecision(18, 2);
            b.HasIndex(x => x.Name).IsUnique(false);
        }
    }
}
