using Microsoft.EntityFrameworkCore;

namespace RestroLogic.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db, CancellationToken ct = default)
        {
            await db.Database.MigrateAsync(ct);
            // Semillas futuras: categorías, menú, mesas, etc.
        }
    }
}
