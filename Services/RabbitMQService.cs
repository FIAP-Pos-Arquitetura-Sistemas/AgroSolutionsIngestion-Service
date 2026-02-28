using AgroSolutions_IngestionService.Models;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace AgroSolutions_IngestionService.Services
{

    public class RabbitMQService : IMessageBusService
    {
        private readonly IConfiguration _config;
        public RabbitMQService(IConfiguration config) => _config = config;

        // Mude para async Task
        public async Task PublishSensorData(SensorData data)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:Host"] ?? "rabbitmq-service",
                UserName = _config["RabbitMQ:User"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync(); // Na v7, CreateModel virou CreateChannelAsync

            await channel.QueueDeclareAsync(queue: "sensor_data_queue",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false);

            var message = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: "",
                                            routingKey: "sensor_data_queue",
                                            body: body);
        }
        public async Task SendToQueue(SensorData data)
        {
            var factory = new ConnectionFactory { HostName = "rabbitmq-service" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: "sensor_data_queue", durable: true);

            var message = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "sensor_data_queue", body: body);
        }
    }
}
