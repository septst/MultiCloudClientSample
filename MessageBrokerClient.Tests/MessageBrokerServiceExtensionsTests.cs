using System.Collections.Generic;
using FluentAssertions;
using MessageBrokerClient.Clients;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace MessageBrokerClient.Tests;

public class MessageBrokerServiceExtensionsTests
{
    [Fact]
    public void RegistersDependenciesCorrectly_WithConfiguredClient()
    {
        // Arrange
        var configDictionary = new Dictionary<string, string>
        {
            { "MessageBroker:Name", "RabbitMq" },
            { "MessageBroker:ConnectionString", "dummy-connection" },
            { "MessageBroker:QueueName", "sample-queue" }
        };
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddInMemoryCollection(configDictionary);

        // Act
        builder.AddMessageBrokerClient();
        using var app = builder.Build();
        var messageBrokerContextService =
            app.Services.GetRequiredService<IMessageBrokerContext>();
        var messageBrokerOptions = app.Services
            .GetRequiredService<IOptions<MessageBrokerOptions>>().Value;

        // Assert
        messageBrokerOptions.Name.Should().Be(MessageBrokerEnum.RabbitMq);
        messageBrokerOptions.ConnectionString.Should().Be("dummy-connection");
        messageBrokerContextService.Should().BeOfType<MessageBrokerContext>();
        messageBrokerContextService.MessageBrokerClient
            .Should().BeOfType<RabbitMqClient>();
    }

    [Fact]
    public void RegistersDependenciesCorrectly_WithoutNotConfiguredClient()
    {
        // Arrange
        var configDictionary = new Dictionary<string, string>
        {
            { "MessageBroker:Name", "NotConfigured" },
            { "MessageBroker:ConnectionString", "dummy-connection" },
            { "MessageBroker:QueueName", "sample-queue" }
        };
        var builder = WebApplication.CreateBuilder();
        builder.Configuration.AddInMemoryCollection(configDictionary);

        // Act
        builder.AddMessageBrokerClient();
        using var app = builder.Build();
        var messageBrokerContextService =
            app.Services.GetRequiredService<IMessageBrokerContext>();
        var messageBrokerOptions = app.Services
            .GetRequiredService<IOptions<MessageBrokerOptions>>().Value;

        // Assert
        messageBrokerOptions.Name.Should().Be(MessageBrokerEnum.NotConfigured);
        messageBrokerOptions.ConnectionString.Should().Be("dummy-connection");
        messageBrokerContextService.Should().BeOfType<MessageBrokerContext>();
        messageBrokerContextService.MessageBrokerClient
            .Should().BeOfType<NotConfiguredClient>();
    }
}