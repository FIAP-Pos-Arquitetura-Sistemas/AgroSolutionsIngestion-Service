using AgroSolutions_IngestionService.Models;
using Microsoft.EntityFrameworkCore;

namespace AgroSolutions_IngestionService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração adicional para precisão de decimais (opcional, mas boa prática)
            modelBuilder.Entity<SensorData>(entity =>
            {
                entity.ToTable("TelemetriaSensores"); // Nome da tabela no Azure SQL
                entity.Property(e => e.Umidade).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Temperatura).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Precipitacao).HasColumnType("decimal(5,2)");
            });
        }
    }
}
