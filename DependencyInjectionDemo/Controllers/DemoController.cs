using Microsoft.AspNetCore.Mvc;
using DependencyInjectionDemo.Services;

namespace DependencyInjectionDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly ISingletonService _singletonService;
        private readonly IScopedService _scopedService;
        private readonly ITransientService _transientService;

        public DemoController(
            ISingletonService singletonService,
            IScopedService scopedService,
            ITransientService transientService)
        {
            _singletonService = singletonService;
            _scopedService = scopedService;
            _transientService = transientService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = new
            {
                Singleton = new
                {
                    Id = _singletonService.GetId(),
                    Message = _singletonService.GetMessage()
                },
                Scoped = new
                {
                    Id = _scopedService.GetId(),
                    Message = _scopedService.GetMessage()
                },
                Transient = new
                {
                    Id = _transientService.GetId(),
                    Message = _transientService.GetMessage()
                },
                Timestamp = DateTime.UtcNow
            };

            return Ok(result);
        }

        [HttpGet("multiple")]
        public IActionResult GetMultiple()
        {
            // This demonstrates how the same service instances are reused within the same request
            var result = new
            {
                FirstCall = new
                {
                    SingletonId = _singletonService.GetId(),
                    ScopedId = _scopedService.GetId(),
                    TransientId = _transientService.GetId()
                },
                SecondCall = new
                {
                    SingletonId = _singletonService.GetId(),
                    ScopedId = _scopedService.GetId(),
                    TransientId = _transientService.GetId()
                },
                Note = "Notice that Singleton and Scoped IDs remain the same, but Transient IDs are different"
            };

            return Ok(result);
        }

        [HttpGet("compare")]
        public IActionResult CompareServices([FromServices] IScopedService scopedService2, [FromServices] ITransientService transientService2)
        {
            // This endpoint demonstrates the key difference between Scoped and Transient
            var result = new
            {
                Explanation = "This endpoint injects additional instances of Scoped and Transient services to show the difference",
                OriginalInstances = new
                {
                    ScopedId = _scopedService.GetId(),
                    TransientId = _transientService.GetId()
                },
                NewInstances = new
                {
                    ScopedId = _scopedService.GetId(),
                    TransientId = _transientService.GetId()
                },
                Analysis = new
                {
                    ScopedSame = _scopedService.GetId() == scopedService2.GetId(),
                    TransientSame = _transientService.GetId() == transientService2.GetId(),
                    ScopedMessage = _scopedService.GetId() == scopedService2.GetId() 
                        ? "✅ Scoped services share the same instance within the same request" 
                        : "❌ Scoped services should share the same instance",
                    TransientMessage = _transientService.GetId() != transientService2.GetId() 
                        ? "✅ Transient services create new instances every time" 
                        : "❌ Transient services should create different instances"
                }
            };

            return Ok(result);
        }

        [HttpGet("request-comparison")]
        public IActionResult RequestComparison()
        {
            // This endpoint helps you compare across different requests
            var result = new
            {
                Message = "Make multiple requests to this endpoint to see how services behave across different requests",
                CurrentRequest = new
                {
                    SingletonId = _singletonService.GetId(),
                    ScopedId = _scopedService.GetId(),
                    TransientId = _transientService.GetId()
                },
                ExpectedBehavior = new
                {
                    Singleton = "Same ID across ALL requests (application lifetime)",
                    Scoped = "Same ID within same request, different ID for different requests",
                    Transient = "Different ID every time, even within same request"
                }
            };

            return Ok(result);
        }
    }
} 