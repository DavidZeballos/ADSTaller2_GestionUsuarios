using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace src.Infrastructure.Messaging
{
    public class RabbitMqService
    {
        private readonly RabbitMqConfig _config;

        public RabbitMqService(RabbitMqConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            Console.Error.WriteLine($"RabbitMQ Host: {_config.Host}, Username: {_config.Username}");
        }

        public void Publish<T>(T eventMessage) where T : class
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _config.Host,
                    UserName = _config.Username,
                    Password = _config.Password
                };
                Console.Error.WriteLine("Attempting to connect to RabbitMQ...");

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                // Declara la cola antes de publicar
                string queueName = "RegisterQueue";
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                Console.Error.WriteLine("Declared queue.");

                // Serializa el mensaje y publica
                var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventMessage));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true; // Marca el mensaje como persistente

                channel.BasicPublish(exchange: string.Empty, // Publica en la cola directamente
                                     routingKey: queueName,
                                     basicProperties: properties,
                                     body: messageBody);

                Console.Error.WriteLine($"Published message to RabbitMQ: {JsonSerializer.Serialize(eventMessage)}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error publishing to RabbitMQ: {ex.Message}");
            }
        }
    }
}
