using Confluent.Kafka;

namespace ConsumerService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _config;
        public Worker(IConfiguration config) => _config = config;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Kafka consumer configuration
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"] ?? "localhost:9092",
                GroupId = "demo-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            // Create a Kafka consumer
            using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
            consumer.Subscribe("demo-topic");

            // Poll for new messages
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(stoppingToken);
                    Console.WriteLine($"Received: {cr.Message.Value}");
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine($"Kafka error: {ex.Error.Reason}");
                }
            }
        }
    }
}
