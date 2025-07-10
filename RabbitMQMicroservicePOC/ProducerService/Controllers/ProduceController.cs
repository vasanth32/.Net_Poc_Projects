using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace ProducerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProduceController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ProduceController(IConfiguration config) => _config = config;

        // POST /produce
        [HttpPost]
        public IActionResult Post([FromBody] string message)
        {
            // Read RabbitMQ config
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_config["RabbitMQ:Port"] ?? "5672"),
                UserName = _config["RabbitMQ:UserName"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest"
            };

            // Connect to RabbitMQ
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue (idempotent)
            channel.QueueDeclare(queue: "demo-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Publish the message
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "", routingKey: "demo-queue", basicProperties: null, body: body);

            return Ok($"Message sent to RabbitMQ: {message}");
        }
    }
} 