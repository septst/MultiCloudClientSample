using MessageBrokerClient.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessageBrokerClient;

public static class MessageBrokerServiceExtensions
{
    public static void AddMessageBrokerClient(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMessageBrokerContext, MessageBrokerContext>();

        builder.Services.AddSingleton(
            _ =>
                builder.Configuration
                    .GetSection(MessageBrokerConfiguration.Position)
                    .Get<MessageBrokerConfiguration>()
        );

        builder.Services.AddSingleton(
            serviceProvider =>
            {
                var config = serviceProvider.GetRequiredService<MessageBrokerConfiguration>();
                IMessageBrokerClient client = config.Name switch
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