
using Task = Todo.Core.Entities.Task;


namespace Todo.Infrastructure.Services;
public class TaskDueDateService : ITaskDueDateService
{
    public bool IsOverdue(Task task, DateTime now)
    {
        if (!task.DueDate.HasValue)
            return false;
        if (task.IsCompleted)
            return false;
        return task.DueDate.Value.Date < now.Date;
    }

    public bool IsDueToday(Task task, DateTime now)
    {
        if (!task.DueDate.HasValue)
            return false;
        if (task.IsCompleted)
            return false;
        return task.DueDate.Value.Date == now.Date;
    }

    public bool IsDueThisWeek(Task task, DateTime now)
    {
        if (!task.DueDate.HasValue)
            return false;
        if (task.IsCompleted)
            return false;

        var startOfWeek = now.Date.AddDays(-(int)now.Date.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7);

        return task.DueDate.Value.Date >= startOfWeek && task.DueDate.Value.Date < endOfWeek;
    }

    public bool IsDueInFuture(Task task, DateTime now)
    {
        if (!task.DueDate.HasValue)
            return false;
        if (task.IsCompleted)
            return false;
        return task.DueDate.Value.Date > now.Date;
    }
}