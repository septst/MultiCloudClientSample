using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MessageBrokerBase.Clients;

public class RabbitMqClient : IMessageBrokerClient
{
    private readonly ILogger<RabbitMqClient> _logger;

    public RabbitMqClient(ILogger<RabbitMqClient> logger)
    {
        _logger = logger;
    }

    public async Task SendMessageAsync(string message)
    {
        _logger.LogInformation("RabbitMQ Client sends message {Message}", message);
        await Task.CompletedTask;
    }
}