using ELfR.Communications.Email;
using ELfR.Communications.Email.Cli.Configuration;
using ELfR.Communications.Email.Configuration;
using ELfR.Communications.Templates;
using ELfR.Communications.Templates.Configuration;
using ELfR.Communications.Templates.Text.ArgumentPlaceholder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;
Console.CancelKeyPress += (object? sender, ConsoleCancelEventArgs e) =>
{
    cancellationTokenSource.Cancel();
};

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
        .AddCommunicationsTemplates()
        .AddEmail(configuration.GetSection("Email"))
        .AddCli(configuration.GetSection("Cli"))
        .BuildServiceProvider();

var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
var templateService = serviceProvider.GetRequiredService<ITemplateService<IReadOnlyDictionary<string, string>>>();
var emailService = serviceProvider.GetRequiredService<IEmailService>();
var emailOptions = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
var cliOptions = serviceProvider.GetRequiredService<IOptions<CliOptions>>().Value;
var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

IReadOnlySet<EmailIdentity> recipientsList = new HashSet<EmailIdentity>();
var subject = string.Empty;
var body = string.Empty;
if (!string.IsNullOrWhiteSpace(cliOptions.RecipientsListPath)
    && !string.IsNullOrWhiteSpace(cliOptions.TemplateWithArgumentsPath))
{
    logger.LogDebug("Reading recipients list from '{recipientsListPath}'.", cliOptions.RecipientsListPath);
    var recipientsListText = await File.ReadAllTextAsync(cliOptions.RecipientsListPath, cancellationToken);
    recipientsList = new HashSet<EmailIdentity>(JsonSerializer.Deserialize<IReadOnlyList<EmailIdentity>>(recipientsListText, jsonSerializerOptions)!);

    logger.LogDebug("Reading template with arguments from '{templateWithArgumentsPath}'.", cliOptions.TemplateWithArgumentsPath);
    var templateWithArgumentsText = await File.ReadAllTextAsync(cliOptions.TemplateWithArgumentsPath, cancellationToken);
    var templateWithArguments = JsonSerializer.Deserialize<TemplateWithArguments>(templateWithArgumentsText, jsonSerializerOptions)!;
    foreach (var recipient in recipientsList)
    {
        try
        {
            var arguments = new Dictionary<string, string>(templateWithArguments.Arguments);
            arguments["recipientName"] = recipient.Name;

            using var subjectTemplateStream = new MemoryStream(Encoding.UTF8.GetBytes(templateWithArguments.Subject));
            using var subjectOutputStream = new MemoryStream();
            await templateService.HydrateAsync(subjectTemplateStream, subjectOutputStream, arguments, cancellationToken);
            using var subjectOutputReader = new StreamReader(subjectOutputStream);
            subject = subjectOutputReader.ReadToEnd().Trim();

            using var bodyTemplateStream = new MemoryStream(Encoding.UTF8.GetBytes(templateWithArguments.Body));
            using var bodyOutputStream = new MemoryStream();
            await templateService.HydrateAsync(bodyTemplateStream, bodyOutputStream, arguments, cancellationToken);
            using var bodyOutputReader = new StreamReader(bodyOutputStream);
            body = bodyOutputReader.ReadToEnd();

            var emailMessage = new EmailMessage(subject, body, true);
            await emailService.SendAsync(recipient, emailMessage, cancellationToken);
            if (recipientsList.Count > 1)
            {
                await Task.Delay(emailOptions.SendInterval);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred.");
        }
    }
}