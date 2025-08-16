using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestroLogic.Domain.Entities;
using RestroLogic.Domain.Enums;

namespace RestroLogic.Infrastructure.Persistence.Configurations
{
    public sealed class TableConfig : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Code).IsRequired().HasMaxLength(10);
            b.HasIndex(x => x.Code).IsUnique();

            // Guarda el enum como string legible (NVARCHAR)
            b.Property(x => x.Status)
             .HasConversion<int>()
             .HasMaxLength(20)
             .HasDefaultValue(TableStatus.Available);
        }
    }
}
