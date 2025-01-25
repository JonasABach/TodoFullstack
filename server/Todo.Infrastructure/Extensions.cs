using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Todo.Core.DTOs.ListDTOs;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Repositories.Cached;
using Todo.Infrastructure.Repositories.DB;
using Todo.Infrastructure.Services;

namespace Todo.Infrastructure;

public static class Extensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRedisCacheService, RedisCachingService>();
    }

    public static void RegisterRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ListRepository>();
        builder.Services.AddScoped<TasksRepository>();
        builder.Services.AddScoped<AccountRepository>();
    }

    public static void RegisterCachingRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAccountRepository, CachedAccountRepository>();
        builder.Services.AddScoped<IRepository<TaskList, AddListDto, UpdateListDto>, CachedListRepository>();
        builder.Services.AddScoped<IRepository<TaskItem, AddTaskDto, UpdateTaskDto>, CachedTasksRepository>();
    }
}