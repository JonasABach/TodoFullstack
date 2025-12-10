
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Todo.Core.Interfaces;
using EntityTask = Todo.Core.Entities.Task;

namespace Todo.Infrastructure.Services;

public class TaskNotificationService : ITaskNotificationService
{
    private readonly ILogger<TaskNotificationService> _logger;

    public TaskNotificationService(ILogger<TaskNotificationService> logger)
    {
        _logger = logger;
    }

    public Task NotifyTaskCreated(EntityTask task, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Task created: {TaskId}, Name: {TaskName}, DueDate: {DueDate}",
            task.Id,
            task.Name,
            task.DueDate);

        return Task.CompletedTask;
    }

    public Task NotifyTaskCompleted(EntityTask task, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Task completed: {TaskId}, Name: {TaskName}, DueDate: {DueDate}",
            task.Id,
            task.Name,
            task.DueDate);

        return Task.CompletedTask;
    }
}