using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Report.Controllers;

[AllowAnonymous]
[Route("[action]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult HealthCheck(string serviceId)
    {
        return serviceId == Environment.GetEnvironmentVariable("ServiceID") ? Ok() : BadRequest();
    }
}