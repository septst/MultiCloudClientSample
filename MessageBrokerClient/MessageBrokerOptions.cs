using System.ComponentModel.DataAnnotations;

namespace MessageBrokerClient;

public class MessageBrokerOptions
{
    public const string Position = "MessageBroker";

    [Required] public string ConnectionString { get; set; } = string.Empty;

    [Required] public MessageBrokerEnum Name { get; set; } = MessageBrokerEnum.NotConfigured;
}