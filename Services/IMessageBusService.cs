using AgroSolutions_IngestionService.Models;

namespace AgroSolutions_IngestionService.Services
{
    public interface IMessageBusService { Task PublishSensorData(SensorData data); }
}
