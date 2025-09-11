using Catalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
        )
        {
            services.AddDbContext<CatalogDbContext>(opt => {
                
                // opt.LogTo(Console.WriteLine, [
                //     DbLoggerCategory.Database.Command.Name
                // ], LogLevel.Information).EnableSensitiveDataLogging();

                opt.UseSqlite(configuration.GetConnectionString("SqliteProduct"));

            });

            // Registrar el servicio de migración
            services.AddScoped<IDatabaseMigrationService, DatabaseMigrationService>();

            return services;
        }

}