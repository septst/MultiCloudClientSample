using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MessageBrokerBase.Clients;

public class NotConfiguredClient : IMessageBrokerClient
{
    private readonly ILogger<NotConfiguredClient> _logger;

    public NotConfiguredClient(ILogger<NotConfiguredClient> logger)
    {
        _logger = logger;
    }

    public async Task SendMessageAsync(string message)
    {
        _logger.LogInformation("NotConfigured Client sends message {Message}", message);
        await Task.CompletedTask;
    }
}