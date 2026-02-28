using AgroSolutions_IngestionService.Models;
using AgroSolutions_IngestionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgroSolutions_IngestionService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngestionController : ControllerBase
    {
        private readonly IMessageBusService _busService;

        public IngestionController(IMessageBusService busService) => _busService = busService;

        [HttpPost]
        public IActionResult Post([FromBody] SensorData data)
        {
            // Regra de Negócio Básica: Validação
            if (string.IsNullOrEmpty(data.TalhaoId.ToString())) return BadRequest("Talhão é obrigatório.");

            // Envia para a fila (Processamento Assíncrono)
            _busService.PublishSensorData(data);

            return Accepted(new { message = "Dados enviados para análise com sucesso!" });
        }
    }
}
