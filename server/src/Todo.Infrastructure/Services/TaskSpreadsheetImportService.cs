using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Todo.Core.Entities;
using Todo.Core.Enums;
using Todo.Core.Interfaces;
using Todo.Infrastructure.DatabaseContexts;
using Task = Todo.Core.Entities.Task;
using Todo.Infrastructure.DatabaseContexts;

namespace Todo.Infrastructure.Services;

public class TaskSpreadsheetImportService : ITaskSpreadsheetImportService
{
    private readonly TodoIdentityContext _context;
    private readonly ILogger<TaskSpreadsheetImportService> _logger;

    public TaskSpreadsheetImportService(
        TodoIdentityContext context,
        ILogger<TaskSpreadsheetImportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ImportResult> ImportAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
    {
        var result = new ImportResult();

        if (stream == null || stream.Length == 0)
        {
            result.Errors.Add("The provided file is empty.");
            return result;
        }

        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheets.First();

        var firstDataRow = 2; // Assuming the first row is the header
        var lastRow = worksheet.LastRowUsed().RowNumber();

        for (int rowNum = firstDataRow; rowNum <= lastRow; rowNum++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            result.TotalRows++;

            var row = worksheet.Row(rowNum);

            try
            {
                var taskName = row.Cell("B").GetString();
                var taskDescription = row.Cell("C").GetString();
                var isCompletedRaw = row.Cell("D").GetString();
                var priorityRaw = row.Cell("E").GetString();
                var listName = row.Cell("G").GetString();
                var listDescription = row.Cell("H").GetString();
                var username = row.Cell("L").GetString();
                var email = row.Cell("M").GetString();

                if (string.IsNullOrWhiteSpace(taskName) || string.IsNullOrWhiteSpace(listName) || (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email)))
                {
                    result.SkippedRows++;
                    result.Errors.Add($"Row {rowNum}: Missing required fields.");
                    continue;
                }

                // Find user by email or username
                var userQuery = _context.Users.AsQueryable();
                User? user = null;
                if (!string.IsNullOrWhiteSpace(email))
                {
                    user = await userQuery.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
                }

                if (user == null && !string.IsNullOrWhiteSpace(username))
                {
                    user = await userQuery.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
                }

                if (user == null)
                {
                    result.SkippedRows++;
                    result.Errors.Add($"Row {rowNum}: User not found.");
                    continue;
                }

                // Find or create task list
                var taskList = await _context.Lists
                    .FirstOrDefaultAsync(tl => tl.UserId == user.Id && tl.Name == listName, cancellationToken);

                if (taskList == null)
                {
                    taskList = new TaskList
                    {
                        Name = listName,
                        Description = listDescription,
                        UserId = user.Id
                    };
                    await _context.Lists.AddAsync(taskList, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Parse isCompleted
                var isCompleted = ParseIsCompleted(isCompletedRaw);
                // Parse priority
                var priority = ParsePriority(priorityRaw);

                // Create and add task
                var task = new Task
                {
                    Name = taskName,
                    Description = taskDescription,
                    IsCompleted = isCompleted,
                    Priority = priority,
                    ListId = taskList.Id
                };
                await _context.Tasks.AddAsync(task, cancellationToken);

                result.ImportedTasks++;
            }
            catch (Exception ex)
            {
                result.SkippedRows++;
                result.Errors.Add($"Row {rowNum}: Error importing task - {ex.Message}");
                _logger.LogError(ex, "Error importing row {RowNum}", rowNum);
            }
        }
        await _context.SaveChangesAsync(cancellationToken);
        return result;
    }

    private static bool ParseIsCompleted(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return false;

        raw = raw.Trim().ToLower();

        if (raw == "true" || raw == "yes" || raw == "1" || raw == "completed")
        {
            return true;
        }
        return false;
    }

    private static TaskPriority? ParsePriority(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;

        raw = raw.Trim();

        if (Enum.TryParse<TaskPriority>(raw, ignoreCase: true, out var parsed))
        {
            return parsed;
        }

        return null;
    }
}