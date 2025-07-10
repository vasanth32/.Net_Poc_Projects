using Microsoft.AspNetCore.Mvc;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        private static bool _isHealthy = true;

        [HttpGet("health")]
        public IActionResult GetHealth()
        {
            if (_isHealthy)
                return Ok(new { status = "Healthy" });
            else
                return StatusCode(500, new { status = "Unhealthy" });
        }

        [HttpPost("toggle-health")]
        public IActionResult ToggleHealth()
        {
            _isHealthy = !_isHealthy;
            return Ok(new { status = _isHealthy ? "Healthy" : "Unhealthy" });
        }
    }
} 