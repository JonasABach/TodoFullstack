using Microsoft.EntityFrameworkCore;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.DatabaseContexts;

namespace Todo.Infrastructure.Services;

public class TaskSummaryService : ITaskSummaryService
{
    private readonly TodoIdentityContext _dbContext;
    private readonly ITaskDueDateService _taskDueDateService;

    public TaskSummaryService(
        TodoIdentityContext dbContext,
        ITaskDueDateService taskDueDateService)
    {
        _dbContext = dbContext;
        _taskDueDateService = taskDueDateService;
    }

    public async Task<DueDateSummaryDto> GetDueDateSummaryAsync(
        CancellationToken cancellationToken = default)
    {
        var tasks = await _dbContext.Tasks.AsNoTracking().ToListAsync(cancellationToken);
        var now = DateTime.UtcNow;

        var summary = new DueDateSummaryDto();

        foreach (var task in tasks)
        {
            if (_taskDueDateService.IsOverdue(task, now))
            {
                summary.OverdueCount++;
            }
            else if (_taskDueDateService.IsDueToday(task, now))
            {
                summary.DueTodayCount++;
            }
            else if (_taskDueDateService.IsDueThisWeek(task, now))
            {
                summary.DueThisWeekCount++;
            }
            else if (_taskDueDateService.IsDueInFuture(task, now))
            {
                summary.DueInFutureCount++;
            }
        }

        return summary;
    }
}