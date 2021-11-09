using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Report.Controllers;

[AllowAnonymous]
[Route("api/[action]")]
public class ReportController : ControllerBase
{
    private readonly History[] _histories =
    {
        new() {ProductId = 2, Count = 1, Custormer = "Ali", TotalPrice = 500},
        new() {ProductId = 6, Count = 2, Custormer = "Taghi", TotalPrice = 2300}
    };

    [HttpGet]
    public IActionResult GetHistory()
    {
        return Ok(_histories);
    }
}