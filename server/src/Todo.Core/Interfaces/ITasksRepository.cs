using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todo.Core.DTOs.TasksDtos;
using TaskEntity = Todo.Core.Entities.Task;

namespace Todo.Core.Interfaces;

/// <summary>
///     Specialized repository interface for task-related operations.
/// </summary>
public interface ITasksRepository : IRepository<TaskEntity, AddTaskDto, UpdateTaskDto>
{
    /// <summary>
    ///     Returns tasks that match the provided filter options.
    /// </summary>
    /// <param name="filter">Filter options for querying tasks.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>A sequence of tasks that satisfy the filter.</returns>
    Task<IEnumerable<TaskEntity>> FilterAsync(
        TaskFilterDto filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Processes due reminders by finding tasks that are not completed,
    ///     have a reminder scheduled at or before the specified time, and
    ///     have not yet had a reminder sent. Marks them as reminded and
    ///     persists the changes.
    /// </summary>
    /// <param name="now">The cutoff time for due reminders.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The list of tasks for which reminders were processed.</returns>
    Task<IReadOnlyList<TaskEntity>> ProcessDueRemindersAsync(
        DateTime now,
        CancellationToken cancellationToken = default);
}