namespace Catalog.Infrastructure.Services;

public interface IDatabaseMigrationService
{
    Task MigrateAsync();
    Task SeedAsync();
}
