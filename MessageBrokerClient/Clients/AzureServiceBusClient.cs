using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MessageBrokerBase.Clients;

public class AzureServiceBusClient : IMessageBrokerClient
{
    private readonly ILogger<AzureServiceBusClient> _logger;

    public AzureServiceBusClient(ILogger<AzureServiceBusClient> logger)
    {
        _logger = logger;
    }

    public async Task SendMessageAsync(string message)
    {
        _logger.LogInformation("Azure Service Bus Client sends message {Message}", message);
        await Task.CompletedTask;
    }
}