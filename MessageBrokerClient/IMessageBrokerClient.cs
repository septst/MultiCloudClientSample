using System.Threading.Tasks;

namespace MessageBrokerClient;

public interface IMessageBrokerClient
{
    Task SendMessageAsync(string message);
}