namespace MessageBrokerBase;

public class MessageBrokerConfiguration
{
    public const string Position = "MessageBrokerEnum";

    public string ConnectionString { get; set; } = string.Empty;
    public MessageBrokerEnum Name { get; set; } = MessageBrokerEnum.NotConfigured;
}