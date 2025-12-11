namespace Todo.Core.Interfaces;

public class ImportResult
{
    public int TotalRows { get; set; }
    public int ImportedTasks { get; set; }
    public int SkippedRows { get; set; }
    public List<string> Errors { get; set; } = new();
}

public interface ITaskSpreadsheetImportService
{
    Task<ImportResult> ImportAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default);
}