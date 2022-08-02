using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageBrokerClient.Clients;

public class AzureServiceBusClient : IMessageBrokerClient
{
    private readonly ILogger<AzureServiceBusClient> _logger;
    private readonly MessageBrokerOptions _messageBrokerOptions;

    public AzureServiceBusClient(
        IOptions<MessageBrokerOptions> messageBrokerOptions,
        ILogger<AzureServiceBusClient> logger)
    {
        _messageBrokerOptions = messageBrokerOptions.Value;
        _logger = logger;
    }

    public async Task SendMessageAsync(string message)
    {
        _logger.LogInformation(
            "{Client} sends message {Message}",
            _messageBrokerOptions.Name,
            message);
        await Task.CompletedTask;
    }
}