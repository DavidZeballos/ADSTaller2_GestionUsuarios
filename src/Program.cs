using Microsoft.EntityFrameworkCore;
using src.Services;
using src.Application.UseCases;
using src.Infrastructure.Persistence;
using src.Domain.Repositories;
using src.Infrastructure.Messaging;
using src.Domain.Events;

var builder = WebApplication.CreateBuilder(args);

// Configuración de RabbitMQ
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>() 
                    ?? throw new InvalidOperationException("RabbitMQ configuration is missing");
builder.Services.AddSingleton(rabbitMqConfig);

// Registro de servicios RabbitMQ
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddSingleton<RabbitMqConsumerService>();

// Registro del RabbitMqConsumerService como BackgroundService
builder.Services.AddHostedService<RabbitMqConsumerService>();

// Registro de casos de uso y dependencias scoped
builder.Services.AddScoped<CreateUserFromEvent>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configuración de gRPC y base de datos
builder.Services.AddGrpc();
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorios y servicios adicionales
builder.Services.AddScoped<GetUserProfile>();
builder.Services.AddScoped<UpdateUserProfile>();

builder.Services.AddScoped<GetUserProgress>();
builder.Services.AddScoped<UpdateUserProgress>();

var app = builder.Build();

// Ejecuta automáticamente las migraciones
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    dbContext.Database.Migrate();
}

app.MapGrpcService<UserGrpcService>();
app.MapGrpcService<ProgressGrpcService>();
app.MapGet("/", () => "User management service is active");
app.Run();
