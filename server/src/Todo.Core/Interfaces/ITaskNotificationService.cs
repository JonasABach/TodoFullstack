using System.Threading;
using System.Threading.Tasks;
using EntityTask = Todo.Core.Entities.Task;

namespace Todo.Core.Interfaces;

public interface ITaskNotificationService
{
    Task NotifyTaskCreated(EntityTask task, CancellationToken cancellationToken = default);
    Task NotifyTaskCompleted(EntityTask task, CancellationToken cancellationToken = default);
}