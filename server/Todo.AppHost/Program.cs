var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("SqlServer", port: 4000)
    .WithImage("mssql/server", "2022-latest")
    .WithContainerName("todo-sqlserver")
    .WithDataVolume("todo-sqlserver-data")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithEnvironment("MSSQL_PID", "Developer")
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer
    .AddDatabase("TodoDb", databaseName: "TodoDb");

var redis = builder
    .AddRedis("Redis", port: 4001)
    .WithImage("redis")
    .WithContainerName("todo-redis")
    .WithDataVolume("todo-redis-data")
    .WithPersistence(TimeSpan.FromMinutes(5))
    .WithRedisInsight();

builder.AddProject<Projects.Todo_Api>("TodoApi", launchProfileName: "https")
    .WithReference(database)
    .WithReference(redis)
    .WaitFor(database)
    .WaitFor(redis)
    .WithExternalHttpEndpoints();

builder.Build().Run();