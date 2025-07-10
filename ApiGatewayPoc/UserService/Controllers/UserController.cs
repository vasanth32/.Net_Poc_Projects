using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        // GET /user or /user/
        [HttpGet("")]
        [HttpGet("/")]
        [HttpGet("{*path}")]
        public IActionResult Get() =>
            Ok("Hello from User Service!");
    }
} 