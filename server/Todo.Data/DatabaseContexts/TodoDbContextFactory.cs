using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Todo.Data.DatabaseContexts;

public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
{
    public TodoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables("ASPNETCORE_ENVIRONMENT")
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<TodoDbContext>()
            .Build();

        var connectionString = configuration.GetConnectionString("TodoDb")!;
        var builder = new DbContextOptionsBuilder<TodoDbContext>();
        builder.UseSqlServer(connectionString);
        return new TodoDbContext(builder.Options);
    }
}