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
}