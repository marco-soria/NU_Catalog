using Catalog.Infrastructure.Services;

namespace Catalog.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseInitializationAsync(this IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();

        // Configurar migraciones basado en configuración
        var autoMigrate = configuration.GetValue<bool>("Database:AutoMigrate", environment.IsDevelopment());
        var autoSeed = configuration.GetValue<bool>("Database:AutoSeed", environment.IsDevelopment());

        if (!autoMigrate && !autoSeed)
        {
            logger.LogInformation("Database initialization skipped. AutoMigrate: {AutoMigrate}, AutoSeed: {AutoSeed}", 
                autoMigrate, autoSeed);
            return;
        }

        try
        {
            logger.LogInformation("Database initialization requested. AutoMigrate: {AutoMigrate}, AutoSeed: {AutoSeed}", 
                autoMigrate, autoSeed);
                
            using var scope = app.ApplicationServices.CreateScope();
            var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
            
            if (autoMigrate)
            {
                await migrationService.MigrateAsync();
            }
            
            if (autoSeed)
            {
                await migrationService.SeedAsync();
            }

            logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Failed to initialize database.");
            
            // En desarrollo, fallar rápido. En producción, continuar sin DB
            if (environment.IsDevelopment())
            {
                logger.LogCritical("Application cannot start in development environment without database.");
                Environment.Exit(1);
            }
            else
            {
                logger.LogWarning("Continuing startup without database initialization in production environment.");
            }
        }
    }
}