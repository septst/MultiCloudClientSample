using System.Threading.Tasks;

namespace MessageBrokerBase;

public interface IMessageBrokerClient
{
    Task SendMessageAsync(string message);
}