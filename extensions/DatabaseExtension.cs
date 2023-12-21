using System.Reflection;
using DbUp;

namespace AHStats.extensions;

public static class DatabaseExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<TContext>>();
        
        logger.LogInformation("Migrating postgresql database.");

        var connection = configuration.GetConnectionString("AHStats");
        
        EnsureDatabase.For.PostgresqlDatabase(connection);

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connection)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            logger.LogError(result.Error, "An error occurred while migrating the postgresql database");
            return host;
        }
        
        logger.LogInformation("Migrated postgresql database.");
        
        return host;
    }
    
}