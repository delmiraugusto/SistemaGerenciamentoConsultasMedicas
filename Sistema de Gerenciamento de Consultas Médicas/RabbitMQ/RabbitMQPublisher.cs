using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.RabbitMQ;
using System.Text;
using System.Text.Json;

public class RabbitMQPublisher
{
    private readonly RabbitMQSettings _settings;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> settings)
    {
        _settings = settings.Value;
    }

    public virtual void Publish<T>(T message, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

}