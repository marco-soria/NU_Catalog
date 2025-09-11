using Catalog.Infrastructure.Services;

namespace Catalog.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            var migrationService = serviceProvider.GetRequiredService<IDatabaseMigrationService>();
            
            logger.LogInformation("Applying database migrations...");
            await migrationService.MigrateAsync();
            
            logger.LogInformation("Applying database seeding...");
            await migrationService.SeedAsync();
            
            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during database initialization");
            throw; // Re-lanzar para que la aplicación no inicie con DB en mal estado
        }
    }
}