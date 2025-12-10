namespace Todo.Core.DTOs.TasksDtos;

public class DueDateSummaryDto
{
    public int OverdueCount { get; set; }
    public int DueTodayCount { get; set; }
    public int DueThisWeekCount { get; set; }
    public int DueInFutureCount { get; set; }
}