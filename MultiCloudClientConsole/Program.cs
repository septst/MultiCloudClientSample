using System;
using MessageBrokerBase;
using MessageBrokerBase.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var startupLogger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

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
        MessageBrokerConfiguration config = serviceProvider.GetRequiredService<MessageBrokerConfiguration>();
        IMessageBrokerClient client = config.Name switch
        {
            MessageBrokerEnum.NotConfigured => ActivatorUtilities.CreateInstance<NotConfiguredClient>(serviceProvider),
            MessageBrokerEnum.AzureServiceBus => ActivatorUtilities.CreateInstance<AzureServiceBusClient>(
                serviceProvider),
            MessageBrokerEnum.RabbitMq => ActivatorUtilities.CreateInstance<RabbitMqClient>(serviceProvider),
            _ => ActivatorUtilities.CreateInstance<NotConfiguredClient>(serviceProvider)
        };
        return client;
    });

var app = builder.Build();

var messageBrokerContextService =
    app.Services.GetRequiredService<IMessageBrokerContext>();
try
{
    await messageBrokerContextService.SendMessageAsync("Hello World!");

    await app.RunAsync();

    Console.ReadKey();
}
catch (Exception exception)
{
    startupLogger.Fatal(exception, "Error occurred during startup");
}