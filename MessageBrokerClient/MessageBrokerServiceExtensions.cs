using MessageBrokerClient.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MessageBrokerClient;

public static class MessageBrokerServiceExtensions
{
    public static void AddMessageBrokerClient(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMessageBrokerContext, MessageBrokerContext>();

        var messageBrokerOptions = builder.Configuration
            .GetSection(MessageBrokerOptions.Position);

        builder.Services
            .AddOptions<MessageBrokerOptions>()
            .Bind(messageBrokerOptions)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddSingleton(
            serviceProvider =>
            {
                var configOptions = serviceProvider.GetRequiredService<IOptions<MessageBrokerOptions>>().Value;
                IMessageBrokerClient client = configOptions.Name switch
                {
                    MessageBrokerEnum.NotConfigured => ActivatorUtilities.CreateInstance<NotConfiguredClient>(
                        serviceProvider),
                    MessageBrokerEnum.AzureServiceBus => ActivatorUtilities.CreateInstance<AzureServiceBusClient>(
                        serviceProvider),
                    MessageBrokerEnum.RabbitMq => ActivatorUtilities.CreateInstance<RabbitMqClient>(serviceProvider),
                    _ => ActivatorUtilities.CreateInstance<NotConfiguredClient>(serviceProvider)
                };
                return client;
            });
    }
}