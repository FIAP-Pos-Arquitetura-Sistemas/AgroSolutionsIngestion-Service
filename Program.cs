using Microsoft.EntityFrameworkCore;
using AgroSolutions_IngestionService.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<RabbitMQWorker>();

var host = builder.Build();
host.Run();