using System.Threading.Tasks;

namespace MessageBrokerBase;

public interface IMessageBrokerContext
{
    IMessageBrokerClient MessageBrokerClient { get; }

    Task SetMessageBrokerClientAsync(IMessageBrokerClient messageBrokerClient);
    Task SendMessageAsync(string message);
}