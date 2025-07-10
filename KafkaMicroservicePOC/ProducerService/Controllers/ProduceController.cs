using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;

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
        public async Task<IActionResult> Post([FromBody] string message)
        {
            // Kafka configuration
            var kafkaConfig = new ProducerConfig { BootstrapServers = _config["Kafka:BootstrapServers"] ?? "localhost:9092" };
            // Create a Kafka producer
            using var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();

            // Send the message to the 'demo-topic' topic
            var result = await producer.ProduceAsync("demo-topic", new Message<Null, string> { Value = message });
            // Return the Kafka offset for learning
            return Ok($"Message sent to Kafka (offset: {result.Offset})");
        }
    }
} 