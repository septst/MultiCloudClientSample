using System.Threading.Tasks;

namespace MessageBrokerClient;

public class MessageBrokerContext : IMessageBrokerContext
{
    public MessageBrokerContext(IMessageBrokerClient messageBrokerClient)
    {
        MessageBrokerClient = messageBrokerClient;
    }

    public IMessageBrokerClient MessageBrokerClient { get; private set; }

    public async Task SetMessageBrokerClientAsync(IMessageBrokerClient messageBrokerClient)
    {
        MessageBrokerClient = messageBrokerClient;
        await Task.CompletedTask;
    }

    public Task SendMessageAsync(string message)
    {
        return MessageBrokerClient.SendMessageAsync(message);
    }
}