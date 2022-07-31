using System;
using MessageBrokerBase;
using MessageBrokerBase.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
builder.Logging.AddJsonConsole();

builder.Services.AddSingleton<IMessageBrokerContext, MessageBrokerContext>();
builder.Services.AddSingleton<IMessageBrokerClient, NotConfiguredClient>();

var app = builder.Build();
var logger = app.Logger;

try
{
    var messageBrokerContextService = app.Services.GetRequiredService<IMessageBrokerContext>();

    using var loggerFactory = LoggerFactory.Create(
        loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Information)
            .AddJsonConsole());
    
    var messageBrokerConfiguration = builder.Configuration
        .GetSection("MessageBroker")
        .Get<MessageBrokerConfiguration>();

    IMessageBrokerClient messageBrokerClient = messageBrokerConfiguration.Name switch
    {
        MessageBrokerEnum.NotConfigured => new NotConfiguredClient(
            loggerFactory.CreateLogger<NotConfiguredClient>()),
        MessageBrokerEnum.AzureServiceBus => new AzureServiceBusClient(
            loggerFactory.CreateLogger<AzureServiceBusClient>()),
        MessageBrokerEnum.RabbitMq => new RabbitMqClient(
            loggerFactory.CreateLogger<RabbitMqClient>()),
        _ => new NotConfiguredClient(loggerFactory.CreateLogger<NotConfiguredClient>())
    };

    await messageBrokerContextService.SetMessageBrokerClientAsync(messageBrokerClient);
    await messageBrokerContextService.SendMessageAsync("Hello World!");

    await app.RunAsync();

    Console.ReadKey();
}
catch (Exception exception)
{
    logger.LogError(exception, "Error occured during startup");
}