using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Entities;

namespace RestroLogic.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
        {
            await db.Database.MigrateAsync(ct);

            if (!db.Tables.Any())
            {
                var tables = Enumerable.Range(1, 12).Select(i => new Table($"M{i}")).ToList();

                // Deja 1-2 ocupadas para pruebas si quieres
                tables[0].MarkOccupied();
                tables[1].MarkCleaning();

                db.Tables.AddRange(tables);
            }

            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product("Hamburguesa Clásica", "Carne 120g, queso, lechuga", 18000m),
                    new Product("Limonada Natural", null, 7000m),
                    new Product("Cheesecake Maracuyá", "Porción", 12000m)
                );
            }

            await db.SaveChangesAsync(ct);
        }
    }
}
