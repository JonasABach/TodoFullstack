using Task = Todo.Core.Entities.Task;

public interface ITaskDueDateService
{
    bool IsOverdue(Task task, DateTime now);
    bool IsDueToday(Task task, DateTime now);
    bool IsDueThisWeek(Task task, DateTime now);
    bool IsDueInFuture(Task task, DateTime now);
}