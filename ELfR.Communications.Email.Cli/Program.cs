using ELfR.Communications.Email;
using ELfR.Communications.Email.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

static string? GetEnvironmentFromArgs(string[] cliArgs)
{
    for (var i = 0; i < cliArgs.Length; i++)
    {
        if (cliArgs[i] == "--environment")
        {
            var environmentPair = cliArgs[i].Split('=');
            return environmentPair[1];
        }
    }
    return null;
}

var environmentName =
    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
    ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
    ?? GetEnvironmentFromArgs(args)
    ?? "Development";
var configuration =
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddEnvironmentVariables()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{environmentName}.json", true)
        .AddCommandLine(args)
        .Build();
var serviceProvider =
    new ServiceCollection()
        .AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole();
        })
        .AddEmail(configuration.GetSection("Email"))
        .BuildServiceProvider();

var emailService = serviceProvider.GetRequiredService<IEmailService>();