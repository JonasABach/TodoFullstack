using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Data.Configurations;
using Todo.Core.Entities;
using Task = Todo.Core.Entities.Task;

namespace Todo.Data.DatabaseContexts;

/// <summary>
///     The database context for the application that extends the IdentityDbContext.
/// </summary>
/// <param name="options">
///     The options to be passed to the base class.
/// </param>
public class TodoIdentityContext(DbContextOptions<TodoIdentityContext> options) : DbContext(options)
{
    /// <summary>
    ///     The DbSet for the User model in the database.
    /// </summary>
    public DbSet<User> Users { get; init; }

    /// <summary>
    ///     The DbSet for the Task model in the database.
    /// </summary>
    public DbSet<Task> Tasks { get; init; }

    /// <summary>
    ///     The DbSet for the TaskList model in the database.
    /// </summary>
    public DbSet<TaskList> Lists { get; init; }

    /// <summary>
    ///     The DbSet for the RefreshToken model in the database.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; init; }

    /// <summary>
    ///     The OnModelCreating method that is called when the model is being created.
    /// </summary>
    /// <param name="modelBuilder">
    ///     The model builder that is used to build the model.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TaskListEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TaskEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
    }
}