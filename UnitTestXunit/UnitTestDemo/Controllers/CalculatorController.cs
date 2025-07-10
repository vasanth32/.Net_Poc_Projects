using Microsoft.AspNetCore.Mvc;
using UnitTestDemo.Services;

namespace UnitTestDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpGet("add")]
        public IActionResult Add([FromQuery] int a, [FromQuery] int b)
        {
            try
            {
                var result = _calculatorService.Add(a, b);
                return Ok(new { result, operation = "add", a, b });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("subtract")]
        public IActionResult Subtract([FromQuery] int a, [FromQuery] int b)
        {
            try
            {
                var result = _calculatorService.Subtract(a, b);
                return Ok(new { result, operation = "subtract", a, b });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("multiply")]
        public IActionResult Multiply([FromQuery] int a, [FromQuery] int b)
        {
            try
            {
                var result = _calculatorService.Multiply(a, b);
                return Ok(new { result, operation = "multiply", a, b });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("divide")]
        public IActionResult Divide([FromQuery] int a, [FromQuery] int b)
        {
            try
            {
                var result = _calculatorService.Divide(a, b);
                return Ok(new { result, operation = "divide", a, b });
            }
            catch (DivideByZeroException)
            {
                return BadRequest(new { error = "Cannot divide by zero" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("is-even/{number}")]
        public IActionResult IsEven(int number)
        {
            try
            {
                var result = _calculatorService.IsEven(number);
                return Ok(new { number, isEven = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("random")]
        public IActionResult GetRandomNumber()
        {
            try
            {
                var result = _calculatorService.GetRandomNumber();
                return Ok(new { randomNumber = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 