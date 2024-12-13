using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using src.Domain.Events;
using src.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace src.Infrastructure.Messaging
{
    public class RabbitMqConsumerService
    {
        private readonly RabbitMqConfig _config;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqConsumerService(RabbitMqConfig config, IServiceProvider serviceProvider)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void StartConsuming()
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.Host,
                UserName = _config.Username,
                Password = _config.Password
            };

            while (true)
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

                    // Salir del bucle si la conexión es exitosa
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not connect to RabbitMQ: {ex.Message}. Retrying...");
                    Thread.Sleep(5000); // Esperar 5 segundos antes de reintentar
                }
            }
        }
    }
}
