using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Todo.Infrastructure.DatabaseContexts;

public class TodoIdentityContextFactory : IDesignTimeDbContextFactory<TodoIdentityContext>
{
  public TodoIdentityContext CreateDbContext(string[] args)
  {
    const string sqlServerConnectionString = "Server=tcp:todofullstackserver.database.windows.net,1433;Initial Catalog=TodoFullstackDB;Persist Security Info=False;User ID=todo_admin;Password=#Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    var optionsBuilder = new DbContextOptionsBuilder<TodoIdentityContext>();
    optionsBuilder.UseSqlServer(sqlServerConnectionString);

    return new TodoIdentityContext(optionsBuilder.Options);
  }
}