using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerService
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _config;
        public Worker(IConfiguration config) => _config = config;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // RabbitMQ connection setup
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_config["RabbitMQ:Port"] ?? "5672"),
                UserName = _config["RabbitMQ:UserName"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest"
            };

            // Fix: Use IConnection for the connection
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declare the queue (idempotent)
            channel.QueueDeclare(queue: "demo-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received: {message}");
            };

            // Start consuming
            channel.BasicConsume(queue: "demo-queue", autoAck: true, consumer: consumer);

            // Keep the task alive
            return Task.Delay(-1, stoppingToken);
        }
    }
}
