using System.Threading.Tasks;

namespace MessageBrokerBase;

public class MessageBrokerContext : IMessageBrokerContext
{
    public MessageBrokerContext(IMessageBrokerClient messageBrokerClient)
    {
        MessageBrokerClient = messageBrokerClient;
    }

    public IMessageBrokerClient MessageBrokerClient { get; private set; }

    public async Task SetStrategyAsync(IMessageBrokerClient messageBrokerClient)
    {
        MessageBrokerClient = messageBrokerClient;
        await Task.CompletedTask;
    }

    public Task SendMessageAsync(string message)
    {
        return MessageBrokerClient.SendMessageAsync(message);
    }
}