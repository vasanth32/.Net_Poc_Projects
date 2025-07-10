using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        // GET /product or /product/
        [HttpGet("")]
        [HttpGet("/")]
        [HttpGet("{*path}")]
        public IActionResult Get() =>
            Ok("Hello from Product Service!");
    }
} 