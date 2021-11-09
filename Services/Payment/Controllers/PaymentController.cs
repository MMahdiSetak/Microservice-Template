using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Payment.Controllers;

[AllowAnonymous]
[Route("api/[action]")]
public class PaymentController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult CheckOut()
    {
        return Ok("Your goods are expensive!!");
    }
}