using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Application.DTO;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.Domain.Entities;
using Sistema_de_Gerenciamento_de_Consultas_Médicas.RabbitMQ;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

public class RabbitMQConsumer : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly IModel _channel;
    private readonly IConnection _connection;

    public RabbitMQConsumer(IOptions<RabbitMQSettings> settings)
    {
        _settings = settings.Value;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "ConsultQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueDeclare(queue: "ConsultUpdateQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);


    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConsult = new EventingBasicConsumer(_channel);
        consumerConsult.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var consult = JsonSerializer.Deserialize<Consult>(message);

            if (consult != null)
            {
                SimulateEmailSending(consult);
            }
        };
        _channel.BasicConsume(queue: "ConsultQueue", autoAck: true, consumer: consumerConsult);

        var consumerUpdateConsult = new EventingBasicConsumer(_channel);
        consumerUpdateConsult.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var consultDTO = JsonSerializer.Deserialize<UpdateConsultDTO>(message);

            if (consultDTO != null)
            {
                SimulateUpdateSending(consultDTO);
            }
        };
        _channel.BasicConsume(queue: "ConsultUpdateQueue", autoAck: true, consumer: consumerUpdateConsult);

        return Task.CompletedTask;
    }

    private void SimulateEmailSending(Consult consult)
    {
        Console.WriteLine($"Email enviado: Consulta Agendada com sucesso, segue informacoes sobre ela," +
            $" Paciente Id = {consult.IdPatient}, Doctor Id = {consult.IdDoctor}, Data da Consulta = {consult.DateTimeQuery}, Descricao da Consulta = {consult.Description} ");
    }

    private void SimulateUpdateSending(UpdateConsultDTO consultDTO)
    {
        Console.WriteLine($"Email enviado: Consulta Atualizada com sucesso, segue as atualizacoes sobre informacoes," +
            $" Descrição da Consulta = {consultDTO.Description}, Data da Consulta = {consultDTO.DateTimeQuery}");
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Dispose();
        base.Dispose();
    }
}