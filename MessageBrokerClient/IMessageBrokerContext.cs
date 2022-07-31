using System.Threading.Tasks;

namespace MessageBrokerBase;

public interface IMessageBrokerContext
{
    IMessageBrokerClient MessageBrokerClient { get; }

    Task SetStrategyAsync(IMessageBrokerClient messageBrokerClient);
    Task SendMessageAsync(string message);
}