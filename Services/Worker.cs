using AgroSolutions_IngestionService.Data;
using AgroSolutions_IngestionService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

public class RabbitMQWorker : BackgroundService
{
    private readonly ILogger<RabbitMQWorker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const string QueueName = "sensor_data_queue";
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMQWorker(ILogger<RabbitMQWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory { HostName = "rabbitmq-service" }; // Nome do serviço no Kubernetes

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var sensorData = JsonSerializer.Deserialize<SensorData>(message);
                if (sensorData != null)
                {
                    await ProcessAndSaveData(sensorData);
                }
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao processar mensagem: {ex.Message}");
                // Em produção, aqui iria para uma Dead Letter Queue
            }
        };

        await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        // Mantém o worker vivo
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessAndSaveData(SensorData data)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Lógica de Alerta de Seca (Requisito do Desafio)
            if (data.Umidade < 30)
            {
                _logger.LogWarning($"ALERTA DE SECA: Talhão {data.TalhaoId} com umidade em {data.Umidade}%!");
                // Aqui você pode inserir em uma tabela de Alertas se houver tempo
            }

            dbContext.SensorData.Add(data);
            await dbContext.SaveChangesAsync();
            _logger.LogInformation($"Dados do sensor {data.SensorId} persistidos no Azure SQL.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync(cancellationToken);
        if (_connection != null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}