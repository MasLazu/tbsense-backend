using Microsoft.EntityFrameworkCore;
using TbSense.Backend.EfCore;
using TbSense.Backend.EfCore.Data;
using TbSense.Backend.Migrator;

try
{
    TbSenseBackendDbContext dbContext = new TbSenseBackendDbContextFactory().CreateDbContext(args);
    await dbContext.Database.MigrateAsync();
    Console.WriteLine($"Migrations applied for {dbContext.GetType().Name}");

    Console.WriteLine("All migrations applied successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
    Environment.Exit(1);
}

Console.WriteLine("Database migration completed. Shutting down...");
