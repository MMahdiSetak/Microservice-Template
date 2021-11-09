using Microsoft.AspNetCore.Mvc;

namespace Product.Controllers;

[ApiController]
[Route("api/[action]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;

    private static Product[] _products =
    {
        new() {Id = 1, Name = "bicycle", Price = 700, Count = 20},
        new() {Id = 2, Name = "laptop", Price = 1000, Count = 10},
        new() {Id = 3, Name = "book", Price = 50, Count = 25},
        new() {Id = 4, Name = "phone", Price = 850, Count = 5}
    };

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), 200)]
    public IActionResult AddProduct(Product request)
    {
        _products = _products.Append(request).ToArray();
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(typeof(Product[]), 200)]
    public IActionResult GetProducts()
    {
        return Ok(_products);
    }
}