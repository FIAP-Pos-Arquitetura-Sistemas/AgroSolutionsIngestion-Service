using System.ComponentModel.DataAnnotations;

namespace AgroSolutions_IngestionService.Models
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SensorId { get; set; } = string.Empty;

        [Required]
        public int TalhaoId { get; set; } // FK lógica para o FarmService

        public double Umidade { get; set; }
        public double Temperatura { get; set; }
        public double Precipitacao { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}