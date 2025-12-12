using Todo.Core.DTOs.TasksDtos;

namespace Todo.Core.Interfaces;

public interface ITaskSummaryService
{
    Task<DueDateSummaryDto> GetDueDateSummaryAsync(string userId, CancellationToken cancellationToken = default);
}