using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using src.Domain.Events;
using src.Application.UseCases;

namespace src.Infrastructure.Messaging
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly RabbitMqConfig _config;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqConsumerService(RabbitMqConfig config, IServiceProvider serviceProvider)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.Username,
                Password = _config.Password
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var connection = factory.CreateConnection();
                    using var channel = connection.CreateModel();

                    string queueName = "RegisterQueue";
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    Console.WriteLine($"Queue declared: {queueName}");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine($"Message received: {message}");

                        try
                        {
                            var userEvent = JsonSerializer.Deserialize<UserCreatedEvent>(message);

                            if (userEvent != null)
                            {
                                using var scope = _serviceProvider.CreateScope();
                                var createUserFromEvent = scope.ServiceProvider.GetRequiredService<CreateUserFromEvent>();

                                await createUserFromEvent.ExecuteAsync(userEvent);
                                Console.WriteLine($"User with ID {userEvent.UserId} created in database.");
                            }
                            else
                            {
                                Console.WriteLine("Failed to deserialize UserCreatedEvent.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing message: {ex.Message}");
                        }
                    };

                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    Console.WriteLine("Waiting for messages...");

                    // Mantener la conexión activa mientras no se detenga el token
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not connect to RabbitMQ: {ex.Message}. Retrying...");
                    await Task.Delay(5000, stoppingToken); // Reintentar después de 5 segundos
                }
            }
        }
    }
}
