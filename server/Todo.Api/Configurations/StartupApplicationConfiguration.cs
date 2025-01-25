using Microsoft.EntityFrameworkCore;
using Todo.Data.DatabaseContexts;

namespace Todo.Api.Configurations;

internal static class StartupApplicationConfiguration
{
    /// <summary>
    ///     Automatically migrates the database at startup.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to use for the migration.
    /// </param>
    private static async Task UseAutoMigrationAtStartup(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var dbContext = services.GetRequiredService<TodoDbContext>();
            await dbContext.Database.MigrateAsync().ConfigureAwait(true);
        }
        catch (Exception e)
        {
            Log.DatabaseMigrationError(logger, e);
        }
    }

    /// <summary>
    ///     Configures the application for development mode.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to configure.
    /// </param>
    public static async Task DevelopmentMode(this WebApplication app)
    {
        app.UseDeveloperExceptionPage();
        app.UseCors(Constants.ClientCrossOriginPolicyDevName);
        await app.UseAutoMigrationAtStartup().ConfigureAwait(true);
    }

    /// <summary>
    ///     Configures the application for production mode.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to configure.
    /// </param>
    public static void ProductionMode(this WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.UseCors(Constants.ClientCrossOriginPolicyProductionName);
    }

    /// <summary>
    ///     Configures the application to Generate API documentation.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to configure.
    /// </param>
    public static void AddDocsGeneration(this WebApplication app)
    {
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "Todo API");
        });
    }
}