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

// Registro de casos de uso
builder.Services.AddScoped<CreateUserFromEvent>();

// Configuración de gRPC y base de datos
builder.Services.AddGrpc();
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorios y servicios adicionales
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<GetUserProfile>();
builder.Services.AddScoped<UpdateUserProfile>();

var app = builder.Build();

// Ejecuta automáticamente las migraciones
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    dbContext.Database.Migrate();
}

// Inicia el consumo de mensajes de RabbitMQ y espera que esté completamente inicializado
var rabbitConsumer = app.Services.GetRequiredService<RabbitMqConsumerService>();
await Task.Run(() => rabbitConsumer.StartConsuming()); // Ahora se espera correctamente

// Simulación de creación de usuario en RabbitMQ (comentada para uso futuro)
/*
var rabbitMqService = app.Services.GetService<RabbitMqService>();
if (rabbitMqService != null)
{
    // Publica un evento de creación de usuario simulado
    var userId = Guid.NewGuid();
    var userCreatedEvent = new UserCreatedEvent(
        userId,
        "John",              // Nombre
        "Doe",               // Primer apellido
        "Smith",             // Segundo apellido
        "12345678-9",        // RUT
        "john.doe@example.com" // Correo electrónico
    );
    rabbitMqService.Publish(userCreatedEvent);

    Console.WriteLine($"Simulated user creation: {userCreatedEvent.UserId}");
}
else
{
    Console.WriteLine("RabbitMqService is not registered.");
}
*/

app.MapGrpcService<UserGrpcService>();
app.MapGet("/", () => "User management service is active");
app.Run();
