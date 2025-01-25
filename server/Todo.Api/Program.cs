using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Todo.Api.Configurations;
using Todo.Core.Validators.Account;
using Todo.Data.DatabaseContexts;
using Todo.Infrastructure;

// Create the builder.
var builder = WebApplication.CreateBuilder(args);

// Initialize all the services.
builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.AddSwaggerService();
builder.AddLoggingService();

builder.AddSqlServerDbContext<TodoDbContext>(connectionName: "TodoDb");
builder.AddRedisDistributedCache(connectionName: "Redis");
builder.Services.AddHealthChecks();

builder.AddFirebaseServices();
builder.AddFirebaseAuthentication();
builder.Services.AddAuthorizationBuilder();
builder.AddCorsService();
builder.RegisterServices();
builder.RegisterRepositories();
builder.RegisterCachingRepositories();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Build the app.
var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
    await app.DevelopmentMode().ConfigureAwait(true);
else
    app.ProductionMode();

app.UseHttpsRedirection();
app.UseHttpLogging();
app.UseRouting();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.AddDocsGeneration();
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePages();
app.MapControllers();

await app.RunAsync().ConfigureAwait(true);

internal sealed partial class Program { }