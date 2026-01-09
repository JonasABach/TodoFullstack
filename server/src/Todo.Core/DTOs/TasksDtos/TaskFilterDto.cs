namespace Todo.Core.DTOs.TasksDtos;
public class TaskFilterDto
{
    public string? ListId { get; set; }
    public string? Search { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DueBefore { get; set; }
}