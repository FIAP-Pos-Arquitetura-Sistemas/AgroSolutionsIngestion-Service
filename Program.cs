using Microsoft.EntityFrameworkCore;
using AgroSolutions_IngestionService.Data;

// Trocamos Host por WebApplication para habilitar o servidor HTTP (Kestrel)
var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Banco de Dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Adiciona os Controllers (Essencial para o Postman funcionar)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Mantém o seu Worker do RabbitMQ rodando em segundo plano
builder.Services.AddHostedService<RabbitMQWorker>();

var app = builder.Build();

// 4. Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// 5. Força a porta 8080 que o Kubernetes espera
app.Run("http://*:8080");