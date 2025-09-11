using Catalog.Api.Extensions;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Configurar migraciones basado en configuración
var autoMigrate = builder.Configuration.GetValue<bool>("Database:AutoMigrate", app.Environment.IsDevelopment());
var autoSeed = builder.Configuration.GetValue<bool>("Database:AutoSeed", app.Environment.IsDevelopment());

if (autoMigrate || autoSeed)
{
    try
    {
        app.Logger.LogInformation("Database initialization requested. AutoMigrate: {AutoMigrate}, AutoSeed: {AutoSeed}", 
            autoMigrate, autoSeed);
            
        using var scope = app.Services.CreateScope();
        var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();
        
        if (autoMigrate)
        {
            await migrationService.MigrateAsync();
        }
        
        if (autoSeed)
        {
            await migrationService.SeedAsync();
        }
    }
    catch (Exception ex)
    {
        app.Logger.LogCritical(ex, "Failed to initialize database. Application cannot start.");
        
        // En desarrollo, fallar rápido. En producción, tal vez continuar sin DB
        if (app.Environment.IsDevelopment())
        {
            Environment.Exit(1);
        }
        else
        {
            app.Logger.LogWarning("Continuing startup without database initialization in production environment.");
        }
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
