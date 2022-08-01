using System;
using MessageBrokerClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.AddMessageBrokerClient();
builder.Configuration.AddJsonFile("appsettings.json");
var startupLogger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

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