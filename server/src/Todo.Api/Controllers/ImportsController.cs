using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.Interfaces;

namespace Todo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ImportsController : ControllerBase
{
    private readonly ITaskSpreadsheetImportService _importService;

    public ImportsController(ITaskSpreadsheetImportService importService)
    {
        _importService = importService;
    }

    [HttpPost("tasks-from-spreadsheet")]
    public async Task<IActionResult> ImportTasksFromSpreadsheet(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        using var stream = file.OpenReadStream();
        var importResult = await _importService.ImportAsync(stream, file.FileName, cancellationToken);
        return Ok(importResult);
    }
}